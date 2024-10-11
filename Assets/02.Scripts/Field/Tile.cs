using Fusion;
using System;
using UnityEngine;

public struct Coord
{
    public int x, y;

    public Coord(int x, int y)
    {
        this.x = x; this.y = y;
    }

    public static bool operator ==(Coord a, Coord b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Coord a, Coord b) => a.x != b.x || a.y != b.y;
}

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

    public ChampionStatus? championStatus { get; set; }

    public Tile DeepCopy()
    {
        Tile newTile = new Tile();
        newTile.tileType = this.tileType;
        newTile.Coordinate = this.Coordinate;
        newTile.DeployPoint = this.DeployPoint;
        newTile.championStatus = this.championStatus;

        return newTile;
    }

    /// <summary>
    /// 타일에 챔피언이 존재하는지 여부
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool IsOccupied(out ChampionStatus? championStatus)
    {
        championStatus = this.championStatus;

        return this.championStatus != null;
    }

    public bool IsOccupied()
    {
        return this.championStatus != null;
    }

    public void DeployChampion(ChampionStatus? champion, Action<Vector2Int, Vector2Int> deployAction)
    {
        this.championStatus = champion;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void DeployChampion(ChampionStatus? championStatus)
    {
        if (championStatus == null)
            return;

        this.championStatus = championStatus;
        //champion.transform.position = DeployPoint;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void Respawn()
    {
        if (championStatus == null)
            return;

        //this.championStatus.transform.position = DeployPoint;
    }

    public void RemoveChampion()
    {
        this.championStatus = null;
    }
}
