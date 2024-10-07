using Fusion;
using System;
using UnityEngine;

public enum FieldType
{
    None = 0,

    WaitField,
    BattleField,
    EnemyField
}

public class FieldTile
{
    public FieldTile(Vector3 position, Vector2Int coord, FieldType fieldType)
    {
        this.DeployPoint = position;
        this.Coordinate = coord;
        this.fieldType = fieldType;
    }
    public FieldType fieldType = FieldType.None;

    public Vector2Int Coordinate { get; private set; }
    public Vector3 DeployPoint { get; private set; }
    private Champion champion = null;

    /// <summary>
    /// 타일에 챔피언이 존재하는지 여부
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool IsOccupied(out Champion champion)
    {
        champion = this.champion;

        return this.champion != null;
    }

    public bool IsOccupied()
    {
        return this.champion != null;
    }

    public void DeployChampion(Champion champion, Action<Vector2Int, Vector2Int> deployAction)
    {
        this.champion = champion;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void DeployChampion(Champion champion)
    {
        this.champion = champion;
        champion.transform.position = DeployPoint;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void RemoveChampion()
    {
        this.champion = null;
    }
}
