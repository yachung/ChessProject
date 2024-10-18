using Fusion;
using System;
using UnityEngine;

public enum TileType
{
    None = -1,

    WaitTile,
    BattleTile,
    EnemyTile
}

[Serializable]
public class Tile
{
    public Tile() { }

    public Tile(Vector3 position, Vector2Int coord, TileType fieldType)
    {
        this.DeployPoint = position;
        this.Coordinate = coord;
        this.tileType = fieldType;
    }
    public TileType tileType = TileType.None;
    public Vector2Int Coordinate { get; private set; }
    public Vector3 DeployPoint { get; private set; }

    public Champion Champion { get; set; }

    //public Tile DeepCopy()
    //{
    //    Tile newTile = new Tile();
    //    newTile.tileType = this.tileType;
    //    newTile.Coordinate = this.Coordinate;
    //    newTile.DeployPoint = this.DeployPoint;
    //    newTile.Champion = this.Champion;

    //    return newTile;
    //}

    /// <summary>
    /// 타일에 챔피언이 존재하는지 여부
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool IsOccupied(out Champion championStatus)
    {
        championStatus = this.Champion;

        return this.Champion != null;
    }

    public bool IsOccupied()
    {
        return this.Champion != null;
    }

    public void DeployChampion(Champion champion, Action<Vector2Int, Vector2Int> deployAction)
    {
        this.Champion = champion;

        Debug.Log($"DeployPoint : {DeployPoint}");

        champion.transform.position = DeployPoint;
    }

    public void DeployChampion(Champion champion, bool isBattle = false)
    {
        if (champion == null)
            return;

        this.Champion = champion;

        Debug.Log($"DeployPoint : {DeployPoint}");

        champion.transform.position = DeployPoint;

        if (isBattle)
            champion.BattleCoord = Coordinate;
        else
            champion.ReadyCoord = Coordinate;
    }

    public void OnMoveComplete()
    {
        if (Champion == null)
            return;

        this.Champion.transform.position = DeployPoint;
        this.Champion.Busy = false;
    }

    public void RespawnChampion()
    {
        if (Champion == null)
            return;

        this.Champion.transform.position = DeployPoint;
    }

    public void RemoveChampion()
    {
        this.Champion = null;
    }
}
