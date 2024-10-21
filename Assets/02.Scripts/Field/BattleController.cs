using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : NetworkBehaviour
{
    public PlayerField playerField;
    private Tile Tile(Vector2Int coord) => playerField.GetBattleTile(coord);

    private bool isBattleFinished = false;

    private readonly (int, int)[] evenDirections = { (0, 1), (1, 0), (0, -1), (-1, -1), (-1, 0), (-1, 1)};
    private readonly (int, int)[] oddDirections = { (1, 1), (1, 0), (1, -1), (0, -1), (-1, 0), (0, 1)};
    private readonly Vector3Int[] cubeDirections =
        {
            new Vector3Int( 0, 1,-1),
            new Vector3Int( 1, 0,-1),
            new Vector3Int( 1,-1, 0),
            new Vector3Int( 0,-1, 1),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(-1, 1, 0)
        };


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
            yield return new WaitForSeconds(0.5f); // 1초에 한 번씩 틱을 처리
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
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Vector2Int currentCoord = new Vector2Int(i, j);

                if (Tile(currentCoord).IsOccupied(out var champion))
                {
                    // 이미 챔피언이 다른 행동을 하고 있는 경우 버퍼에 추가하지 않음.
                    if (champion.IsMovementBusy || champion.IsAttack)
                        continue;

                    // 공격 범위 내에서 적 유닛을 찾음
                    Tile targetTile = FindInRangeTarget(currentCoord, champion.status.Range, target => target.Champion != null && target.Champion.Object.InputAuthority != champion.Object.InputAuthority);

                    if (targetTile != null)
                    {
                        attackBuffers.Add((targetTile.Coordinate, currentCoord));
                    }
                    else
                    {
                        // 근처의 적 유닛을 찾아 추격
                        Tile nearestTargetTile = FindNearestTarget(currentCoord, champion.Object.InputAuthority);

                        if (nearestTargetTile != null)
                        {
                            Vector2Int targetCoord = nearestTargetTile.Coordinate;

                            // 타겟 방향으로 한 칸씩 이동
                            Vector2Int nextStepCoord = GetNextStepTowards(currentCoord, targetCoord);
                            if (nextStepCoord != currentCoord && Tile(nextStepCoord).Champion == null)
                            {
                                // 이동할 좌표가 이미 버퍼에 등록되어 있는지 확인 후 추가
                                if (movementBuffers.TryGetValue(nextStepCoord, out Vector2Int coord))
                                {
                                    Champion existingChampion = Tile(coord).Champion;
                                    Champion movingChampion = Tile(currentCoord).Champion;

                                    // 속도가 더 빠른 유닛이 이동하도록 결정
                                    if (movingChampion.status.Speed > existingChampion.status.Speed)
                                    {
                                        movementBuffers[nextStepCoord] = currentCoord;
                                    }
                                }
                                else
                                {
                                    movementBuffers.Add(nextStepCoord, currentCoord);
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

    private async void MovementUnit(Vector2Int nextStepCoord, Vector2Int previousCoord)
    {
        if (Tile(previousCoord) == null || !Tile(previousCoord).IsOccupied())
            return;

        Tile(nextStepCoord).Champion = Tile(previousCoord).Champion;
        Tile(previousCoord).RemoveChampion();
        Champion champion = Tile(nextStepCoord).Champion;

        Debug.Log($"MoveLog {champion.Object.Id}{champion.Object.InputAuthority} is {previousCoord} to {nextStepCoord}");

        champion.IsMovementBusy = true;
        Vector3 targetPosition = Tile(nextStepCoord).DeployPoint;

        float distance;
        while (Tile(nextStepCoord).Champion != null)
        {
            distance = MoveToward(champion.transform, targetPosition, champion.status.Speed);
            if (distance < 0.5f)
                break;

            RotateToward(champion.transform, targetPosition, 180f);

            await UniTask.Yield();
        }

        Tile(nextStepCoord).OnMoveComplete();
    }

    private void RotateToward(Transform selfTransform, Vector3 destination, float rotateSpeed)
    {
        //Quaternion targetRotation = Quaternion.LookRotation(destination - selfTransform.position);

        Vector3 direction = destination - selfTransform.position;

        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);
        if (directionXZ != Vector3.zero)
        {
            selfTransform.rotation = Quaternion.RotateTowards(
                selfTransform.rotation,
                Quaternion.LookRotation(directionXZ),
                rotateSpeed * Runner.DeltaTime
                );
        }
    }

    private float MoveToward(Transform selfTransform, Vector3 destination, float speed)
    {
        selfTransform.position = Vector3.MoveTowards(selfTransform.position, destination, speed * Runner.DeltaTime);

        return Vector3.Distance(selfTransform.position, destination); 
    }

    //private IEnumerator MoveChampion(Champion champion, Vector3 targetPosition, Action completeCallback)
    //{
    //    while (Vector3.Distance(champion.transform.position, targetPosition) > 0.5f && !isBattleFinished)
    //    {
    //        champion.transform.position = Vector3.MoveTowards(champion.transform.position, targetPosition, champion.status.Speed * Runner.DeltaTime);
    //        yield return null;
    //    }

    //    completeCallback?.Invoke();
    //}

    //private async UniTask MoveChampion(Champion champion, Vector3 targetPosition, Action completeCallback)
    //{
    //    while (Vector3.Distance(champion.transform.position, targetPosition) > 0.5f && !isBattleFinished)
    //    {
    //        champion.transform.position = Vector3.MoveTowards(champion.transform.position, targetPosition, champion.status.Speed * Runner.DeltaTime);
    //        await UniTask.Yield();
    //    }

    //    completeCallback?.Invoke();
    //}

    //private void AttackUnit(Vector2Int target, Vector2Int source)
    //{
    //    if (Tile(target).Champion == null || Tile(source).Champion == null)
    //        return;

    //    Tile(source).Champion.IsAttack = true;

    //    // 타겟 유닛에게 데미지 적용
    //    Tile(target).Champion.Damage(Tile(source).Champion.status.AttackPower);

    //    if (Tile(target).Champion.RemainHp <= 0)
    //    {
    //        Tile(target).RemoveChampion();
    //    }

    //    if (Tile(target).Champion == null)
    //    {
    //        Tile(source).Champion.IsAttack = false;
    //    }
    //}

    private async void AttackUnit(Vector2Int target, Vector2Int source)
    {
        if (Tile(target).Champion == null || Tile(source).Champion == null)
            return;

        Champion attacker = Tile(source).Champion;
        Champion defender = Tile(target).Champion;

        // 챔피언이 이미 공격 중인 경우 중복 실행 방지
        if (attacker.IsAttack)
            return;

        attacker.IsAttack = true;

        // 적 방향으로 회전 및 공격 애니메이션 실행
        Vector3 targetPosition = Tile(target).DeployPoint;

        float attackDelay = 1.0f / attacker.status.AttackSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < attackDelay)
        {
            RotateToward(attacker.transform, targetPosition, 360f); // 적을 바라보도록 지속적으로 회전
            elapsedTime += Runner.DeltaTime;
            await UniTask.Yield();
        }

        // 타겟 유닛에게 데미지 적용
        if (defender != null && defender.RemainHp > 0) // 타겟이 아직 살아있는지 확인
        {
            defender.Damage(attacker.status.AttackPower);

            if (defender.RemainHp <= 0)
            {
                Tile(target).RemoveChampion();
            }
        }

        attacker.IsAttack = false;
    }

    public Tile FindInRangeTarget(Vector2Int source, int radius, Func<Tile, bool> condition)
    {
        Vector3Int center = OffsetToCube(source);
        Tile target;

        for (int k = 1; k <= radius; ++k)
        {
            var hex = Cube_Add(center, Cube_Scale(cubeDirections[4], k));

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < k; ++j)
                {
                    target = Tile(CubeToOffset(hex));

                    if (target == null)
                        continue;

                    if (condition.Invoke(target))
                    {
                        return target;
                    }

                    hex = Cube_Neighbor(hex, i);
                }
            }
        }

        return null;
    }

    public Vector3Int Cube_Scale(Vector3Int hex, int factor)
    {
        return new Vector3Int(hex.x * factor, hex.y * factor, hex.z * factor);
    }

    public Vector3Int Cube_Add(Vector3Int hex, Vector3Int vec)
    {
        return hex + vec;
    }

    public Vector3Int Cube_Neighbor(Vector3Int cube, int index)
    {
        return Cube_Add(cube, cubeDirections[index]);
    }

    // 오프셋 좌표계에서 가장 가까운 타겟을 찾는 함수
    public Tile FindNearestTarget(Vector2Int source, PlayerRef playerRef)
    {
        Tile nearestTile = null;
        int nearestDistance = int.MaxValue;

        for (int i = 0; i <= 7; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Tile target = Tile(new Vector2Int(i, j));

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

        // 이동할수 있는 타일이 없다면 현위치에 멈춤.
        Vector2Int result = current;
        
        foreach (var direction in GetDirections(current.y))
        {
            Vector2Int neighbor = new Vector2Int(current.x + direction.Item1, current.y + direction.Item2);

            if (Tile(neighbor) == null)
                continue;

            if (Tile(neighbor).IsOccupied())
                continue;

            if (distance > GetHexDistance(target, neighbor))
            {
                distance = GetHexDistance(target, neighbor);
                result = neighbor;
            }
        }

        return result;
    }

    /// <summary>
    /// offset 좌표계를 Cube 좌표계로 변환
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    private Vector3Int OffsetToCube(Vector2Int hex)
    {
        var q = hex.x - (hex.y - (hex.y & 1)) / 2;
        var r = hex.y;

        return new Vector3Int(q, r, -q - r);
    }

    private Vector2Int CubeToOffset(Vector3Int hex)
    {
        var x = hex.x + (hex.y - (hex.y & 1)) / 2;
        var y = hex.y;

        return new Vector2Int(x, y);
    }

    public int GetHexDistance(Vector2Int a, Vector2Int b)
    {
        // Convert a and b from Odd-r offset to cube coordinates.
        Vector3Int distance = OffsetToCube(a) - OffsetToCube(b);

        return Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z));
    }

    public bool IsBattleOver(bool forcedEnd = false)
    {
        if (forcedEnd)
            return true;

        Dictionary<PlayerRef, bool> layersPresent = new Dictionary<PlayerRef, bool>();

        for (int i = 0; i <= 7; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Tile target = Tile(new Vector2Int(i, j));
                if (target.Champion != null)
                {
                    layersPresent.TryAdd(target.Champion.Object.InputAuthority, true);
                }
            }
        }

        return layersPresent.Count != 2;
    }
}
