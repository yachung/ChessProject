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
            SimulationTick();  // 한 틱 처리
        }
    }

    public bool SimulationTick()
    {
        bool isFinished = false;

        // 결과적으로 한 틱에 Target 좌표로 향할 객체는 하나뿐이어야 하므로 Key 값.
        // (Key : Target, Value : Current)
        Dictionary<Vector2Int, Vector2Int> movementBuffers = new Dictionary<Vector2Int, Vector2Int>();

        List<(Vector2Int target, Vector2Int source)> attackBuffers = new List<(Vector2Int, Vector2Int)>();

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Champion champion = Tiles[i, j].championStatus;

                if (champion != null)
                {
                    Vector2Int currentCoord = new Vector2Int(i, j);

                    // 공격 범위 내에서 다른 레이어의 유닛을 찾음
                    // 타겟 타일에 챔피언이 존재하고 현재 타일과 InputAuthority가 다른경우 리턴
                    Tile targetTile = FindNodeInRange(currentCoord, champion.Range, tile => tile.championStatus != null && tile.championStatus.Object.InputAuthority != champion.Object.InputAuthority);

                    if (targetTile != null)
                    {
                        attackBuffers.Add((targetTile.Coordinate, new Vector2Int(i, j)));
                    }
                    else
                    {
                        // todo -> Find nearest different layer unit and chase 1 index meter to the unit.
                        Tile nearestTargetTile = FindNearestTarget(currentCoord, champion.Object.InputAuthority);

                        if (nearestTargetTile != null)
                        {
                            Vector2Int targetCoord = nearestTargetTile.Coordinate;

                            // 한 칸씩 이동 (x와 y 중 먼저 이동 가능한 방향으로 이동)
                            Vector2Int newCoord = GetNextStepTowards(currentCoord, targetCoord);
                            if (newCoord != currentCoord && Tiles[newCoord.x, newCoord.y].championStatus == null)
                            {
                                // 유닛 이동
                                //nodes[newCoord.x, newCoord.y].unit = unit;
                                //nodes[i, j].unit = null; // 기존 위치에서 유닛 제거
                                //KeyValuePair<Coord, Coord> buffer = (newCoord, new Coord(i, j));

                                if (movementBuffers.TryGetValue(newCoord, out Vector2Int coord))
                                {
                                    Champion alreadyUnit = Tiles[coord.x, coord.y].championStatus;
                                    Champion newUnit = Tiles[i, j].championStatus;

                                    if (newUnit.Speed > alreadyUnit.Speed)
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

        foreach (var buffer in movementBuffers)
        {
            MovementUnit(buffer.Key, buffer.Value);
        }

        foreach (var buffer in attackBuffers)
        {
            AttackUnit(buffer.target, buffer.source);
        }

        if (IsBattleOver())
        {
            isFinished = true;
        }

        return isFinished;
    }

    private void MovementUnit(Vector2Int newCoord, Vector2Int currentCoord)
    {
        Champion champion = Tiles[currentCoord.x, currentCoord.y].championStatus;

        if (champion == null)
            return;

        // 유닛의 자연스러운 이동 구현
        StartCoroutine(MoveChampion(champion, Tiles[newCoord.x, newCoord.y].DeployPoint));

        // 타일 데이터 업데이트
        Tiles[newCoord.x, newCoord.y].championStatus = champion;
        Tiles[currentCoord.x, currentCoord.y].championStatus = null;
    }

    private IEnumerator MoveChampion(Champion champion, Vector3 targetPosition)
    {
        //float speed = 2.0f; // 이동 속도
        while (Vector3.Distance(champion.transform.position, targetPosition) > 0.1f)
        {
            champion.transform.position = Vector3.MoveTowards(champion.transform.position, targetPosition, champion.ChampionStatus.Speed * Time.deltaTime);
            yield return null;
        }
        champion.transform.position = targetPosition; // 이동 완료 후 정확히 위치
    }

    private void AttackUnit(Vector2Int target, Vector2Int source)
    {
        Champion targetUnit = Tiles[target.x, target.y].championStatus;
        Champion sourceUnit = Tiles[source.x, source.y].championStatus;

        if (targetUnit == null || sourceUnit == null)
            return;

        // 공격 대상이 있으면 공격 수행
        targetUnit.Damage(sourceUnit.ChampionStatus.AttackPower);

        if (targetUnit.ChampionStatus.HealthPoint <= 0)
        {
            Tiles[target.x, target.y].championStatus = null; // 유닛이 사망하면 제거
            Destroy(targetUnit.gameObject);
        }
    }

    public Tile FindNodeInRange(Vector2Int coord, int range, Func<Tile, bool> condition)
    {
        // 맨해튼 거리 내의 좌표만 탐색
        for (int dx = -range; dx <= range; dx++)
        {
            int remainingRange = range - Math.Abs(dx);

            for (int dy = -remainingRange; dy <= remainingRange; dy++)
            {
                int x = coord.x + dx;
                int y = coord.y + dy;

                // 경계 체크 (보드의 범위를 벗어났는지 확인)
                if (x < 0 || x >= Tiles.GetLength(0) || y < 0 || y >= Tiles.GetLength(1))
                    continue;

                Tile tile = Tiles[x, y];

                // 주어진 조건에 맞는 노드가 있는지 확인
                if (condition.Invoke(tile))
                {
                    return tile;
                }
            }
        }

        // 조건을 만족하는 노드가 없는 경우 null을 반환
        return null;
    }
    public Tile FindNearestTarget(Vector2Int coord, PlayerRef layer)
    {
        Tile nearestTile = null;
        int nearestDistance = int.MaxValue;

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Tile tile = Tiles[i, j];

                if (tile.championStatus != null && tile.championStatus.Object.InputAuthority != layer)
                {
                    int distance = Math.Abs(coord.x - i) + Math.Abs(coord.y - j); // 맨해튼 거리 계산

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTile = tile;
                    }
                }
            }
        }

        return nearestTile;
    }

    // 타겟 방향으로 한 칸 이동하는 함수
    public Vector2Int GetNextStepTowards(Vector2Int current, Vector2Int target)
    {
        int dx = target.x - current.x;
        int dy = target.y - current.y;

        Vector2Int moveX = new Vector2Int(current.x + Math.Sign(dx), current.y);
        Vector2Int moveY = new Vector2Int(current.x, current.y + Math.Sign(dy));


        //// x 방향 또는 y 방향으로 한 칸 이동
        //if (Math.Abs(dx) > Math.Abs(dy))
        //{
        //    moveX = new Coord(current.x + Math.Sign(dx), current.y);
        //}
        //else if (Math.Abs(dy) > 0)
        //{
        //    moveY = new Coord(current.x, current.y + Math.Sign(dy));
        //}

        if (Tiles[moveX.x, moveX.y].championStatus == null)
        {
            return moveX;
        }
        else if (Tiles[moveY.x, moveY.y].championStatus == null)
        {
            return moveY;
        }

        //nodes[newCoord.x, newCoord.y].unit == null
        // 현재 위치가 타겟 위치와 같다면 이동하지 않음

        return current;
    }

    public bool IsBattleOver()
    {
        Dictionary<PlayerRef, bool> layersPresent = new Dictionary<PlayerRef, bool>();

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Tile tile = Tiles[i, j];
                if (tile.championStatus != null)
                {
                    layersPresent.TryAdd(tile.championStatus.Object.InputAuthority, true);
                    //layersPresent[tile.champion.layer] = true; // 해당 레이어의 유닛이 존재함
                }
            }
        }

        // 두 개 이상의 레이어가 없으면 전투가 끝남
        return layersPresent.Count != 2;
    }
}
