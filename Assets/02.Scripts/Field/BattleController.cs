using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : NetworkBehaviour
{
    public PlayerField playerField;
    private Tile[,] Tiles => playerField.Tiles;

    private bool isBattleFinished = false;

    private readonly (int, int)[] evenDirections = { (-1, 1), (0, 1), (1, 0), (0, -1), (-1, -1), (-1, 0) };
    private readonly (int, int)[] oddDirections = { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, 0) };

    private (int, int)[] GetDirections(int y)
    {
        return y % 2 == 0 ? evenDirections : oddDirections;
    }

    public void StartBattle()
    {
        // 전투를 시작하는 로직 (틱 기반 전투 처리 시작)
        isBattleFinished = false;
        StartCoroutine(SimulationLoop());
    }

    private IEnumerator SimulationLoop()
    {
        while (!isBattleFinished)
        {
            yield return new WaitForSeconds(1f); // 1초에 한 번씩 틱을 처리
            isBattleFinished = SimulationTick();  // 한 틱 처리
        }
    }

    public void BattleEnd()
    {
        isBattleFinished = true;
        StopAllCoroutines();
    }

    public bool SimulationTick()
    {
        bool isFinished = false;

        // 타일의 이동과 공격을 처리할 버퍼
        Dictionary<Vector2Int, Vector2Int> movementBuffers = new Dictionary<Vector2Int, Vector2Int>();
        List<(Vector2Int target, Vector2Int source)> attackBuffers = new List<(Vector2Int, Vector2Int)>();

        // 모든 타일을 순회하며 각 챔피언의 이동 및 공격을 처리
        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                if (Tiles[i, j].IsOccupied(out var champion))
                {
                    Vector2Int currentCoord = new Vector2Int(i, j);

                    // 공격 범위 내에서 적 유닛을 찾음
                    Tile targetTile = FindNodeInRange(currentCoord, champion.Range, target => target.Champion != null && target.Champion.Object.InputAuthority != champion.Object.InputAuthority);

                    if (targetTile != null)
                    {
                        attackBuffers.Add((targetTile.Coordinate, new Vector2Int(i, j)));
                    }
                    else
                    {
                        // 근처의 적 유닛을 찾아 추격
                        Tile nearestTargetTile = FindNearestTarget(currentCoord, champion.Object.InputAuthority);

                        if (nearestTargetTile != null)
                        {
                            Vector2Int targetCoord = nearestTargetTile.Coordinate;

                            // 타겟 방향으로 한 칸씩 이동
                            Vector2Int newCoord = GetNextStepTowards(currentCoord, targetCoord);
                            if (newCoord != currentCoord && Tiles[newCoord.x, newCoord.y].Champion == null)
                            {
                                // 이동할 타일이 비어있는지 확인 후 이동
                                if (movementBuffers.TryGetValue(newCoord, out Vector2Int coord))
                                {
                                    Champion existingChampion = Tiles[coord.x, coord.y].Champion;
                                    Champion movingChampion = Tiles[i, j].Champion;

                                    // 속도가 더 빠른 유닛이 이동하도록 결정
                                    if (movingChampion.Speed > existingChampion.Speed)
                                    {
                                        movementBuffers[newCoord] = new Vector2Int(i, j);
                                    }
                                }
                                else
                                {
                                    movementBuffers.Add(newCoord, new Vector2Int(i, j));
                                }
                            }
                        }
                    }
                }
            }
        }

        // 이동 및 공격 처리
        foreach (var buffer in movementBuffers)
        {
            MovementUnit(buffer.Key, buffer.Value);
        }

        foreach (var buffer in attackBuffers)
        {
            AttackUnit(buffer.target, buffer.source);
        }

        // 전투 종료 조건 체크
        if (IsBattleOver())
        {
            isFinished = true;
        }

        return isFinished;
    }

    private void MovementUnit(Vector2Int newCoord, Vector2Int currentCoord)
    {
        Champion champion = Tiles[currentCoord.x, currentCoord.y].Champion;

        if (champion == null)
            return;

        // 유닛을 타겟 위치로 이동
        StartCoroutine(MoveChampion(champion, Tiles[newCoord.x, newCoord.y].DeployPoint));

        Debug.Log($"MoveLog {champion.Object.InputAuthority} is {currentCoord} to {newCoord}");

        // 타일 데이터 업데이트
        Tiles[newCoord.x, newCoord.y].DeployChampion(champion, true);
        Tiles[currentCoord.x, currentCoord.y].RemoveChampion();
    }

    private IEnumerator MoveChampion(Champion champion, Vector3 targetPosition)
    {
        while (Vector3.Distance(champion.transform.position, targetPosition) > 0.5f || !isBattleFinished)
        {
            champion.transform.position = Vector3.MoveTowards(champion.transform.position, targetPosition, champion.Speed * Time.deltaTime);
            yield return null;
        }
    }

    private void AttackUnit(Vector2Int target, Vector2Int source)
    {
        Champion targetUnit = Tiles[target.x, target.y].Champion;
        Champion sourceUnit = Tiles[source.x, source.y].Champion;

        if (targetUnit == null || sourceUnit == null)
            return;

        // 타겟 유닛에게 데미지 적용
        targetUnit.Damage(sourceUnit.AttackPower);

        if (targetUnit.HealthPoint <= 0)
        {
            playerField[target].RemoveChampion();
            Destroy(targetUnit.gameObject);
        }
    }

    // 오프셋 좌표계를 사용하여 범위 내에서 타겟을 찾는 함수
    public Tile FindNodeInRange(Vector2Int source, int range, Func<Tile, bool> condition)
    {
        for (int dx = -range; dx <= range; dx++)
        {
            int dyMin = Mathf.Max(-range, -dx - range);
            int dyMax = Mathf.Min(range, -dx + range);

            for (int dy = dyMin; dy <= dyMax; dy++)
            {
                int x = source.x + dx;
                int y = source.y + dy;

                if (x < 0 || x >= Tiles.GetLength(0) || y < 0 || y >= Tiles.GetLength(1))
                    continue;

                Tile target = Tiles[x, y];

                if (condition.Invoke(target))
                {
                    return target;
                }
            }
        }

        return null;
    }

    // 오프셋 좌표계에서 가장 가까운 타겟을 찾는 함수
    public Tile FindNearestTarget(Vector2Int source, PlayerRef playerRef)
    {
        Tile nearestTile = null;
        int nearestDistance = int.MaxValue;

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Tile target = Tiles[i, j];

                if (target.Champion != null && target.Champion.Object.InputAuthority != playerRef)
                {
                    int distance = GetHexDistance(source, new Vector2Int(i, j)); // 오프셋 좌표계에서 거리 계산

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTile = target;
                    }
                }
            }
        }

        return nearestTile;
    }

    // 오프셋 좌표계에서 한 칸 이동하는 함수
    public Vector2Int GetNextStepTowards(Vector2Int current, Vector2Int target)
    {
        int distance = int.MaxValue;
        Vector2Int result = current;
        
        foreach (var direction in GetDirections(current.y))
        {
            Vector2Int coord = new Vector2Int(current.x + direction.Item1, current.y + direction.Item2);

            if (playerField[coord] == null)
                continue;

            if (playerField[coord].IsOccupied())
                continue;

            if (distance > GetHexDistance(target, coord))
            {
                distance = GetHexDistance(target, coord);
                result = coord;
            }
        }

        return result;
    }

    // 오프셋 좌표계에서 두 타일 간의 거리 계산
    private int GetHexDistance(Vector2Int a, Vector2Int b)
    {
        int dx = b.x - a.x;
        int dy = b.y - a.y;

        if (a.y % 2 == 0)
        {
            dy -= dx / 2;
        }
        else
        {
            dy += dx / 2;
        }

        return Mathf.Abs(dx) + Mathf.Abs(dy);
    }

    public bool IsBattleOver(bool forcedEnd = false)
    {
        if (forcedEnd)
            return true;

        Dictionary<PlayerRef, bool> layersPresent = new Dictionary<PlayerRef, bool>();

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Tile tile = Tiles[i, j];
                if (tile.Champion != null)
                {
                    layersPresent.TryAdd(tile.Champion.Object.InputAuthority, true);
                }
            }
        }

        return layersPresent.Count != 2;
    }
}
