using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class ChampionController : NetworkBehaviour
{
    public Animator Animator { get; private set; }
    public Champion Target { get; private set; }

    private readonly (int, int)[] directions = { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, 0) };

    public List<FieldTile> FindEnemiesInRange(FieldTile startTile, int range, Dictionary<(int, int), FieldTile> hexGrid)
    {
        List<FieldTile> enemiesInRange = new List<FieldTile>();
        Queue<(FieldTile tile, int distance)> queue = new Queue<(FieldTile, int)>();
        HashSet<FieldTile> visited = new HashSet<FieldTile>();

        queue.Enqueue((startTile, 0));
        visited.Add(startTile);

        while (queue.Count > 0)
        {
            var (currentTile, distance) = queue.Dequeue();

            if (distance > range) continue; // 범위를 초과하면 탐색 중단

            if (currentTile.IsOccupied(out Champion champion))
            {
                if (champion.Object.InputAuthority != Object.InputAuthority)
                {

                    enemiesInRange.Add(currentTile); // 적을 찾음
                }
            }

            // 6방향으로 탐색
            foreach (var direction in directions)
            {
                (int dx, int dy) = direction;
                var neighborCoord = (currentTile.Coordinate.x + dx, currentTile.Coordinate.y + dy);

                if (hexGrid.ContainsKey(neighborCoord))
                {
                    var neighborTile = hexGrid[neighborCoord];
                    if (!visited.Contains(neighborTile))
                    {
                        visited.Add(neighborTile);
                        queue.Enqueue((neighborTile, distance + 1));
                    }
                }
            }
        }

        return enemiesInRange;
    }

    public ChampionController (Animator animator)
    {
        this.Animator = animator;
    }

    // 탐색
    public bool EnemySearch(out Champion target)
    {
        target = Target;

        return false;
    }

    public bool InRangeCheck()
    {
        return false;
    }

    public bool IsAttackable(Champion target)
    {
        return false;
    }

    private bool IsDead(Champion target)
    {
        return false;
    }

    private void MoveNextTile()
    {

    }

    private void Attack()
    {

    }
}
