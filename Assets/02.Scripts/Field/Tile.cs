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
        this.championStatus = default; // 기본값으로 초기화
    }

    public TileType tileType = TileType.None;
    public Vector2Int Coordinate { get; private set; }
    public Vector3 DeployPoint { get; private set; }

    public ChampionStatus championStatus { get; set; }

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
    public bool IsOccupied(out ChampionStatus championStatus)
    {
        championStatus = this.championStatus;

        return !this.championStatus.Equals(default(ChampionStatus));
    }

    public bool IsOccupied()
    {
        return !championStatus.Equals(default(ChampionStatus));
    }

    public void DeployChampion(ChampionStatus champion, Action<Vector2Int, Vector2Int> deployAction)
    {
        this.championStatus = champion;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void DeployChampion(ChampionStatus championStatus)
    {
        this.championStatus = championStatus;
        //champion.transform.position = DeployPoint;

        Debug.Log($"DeployPoint : {DeployPoint}");

        //deployAction?.Invoke(champion, deployPoint);
        //champion.transform.position = deployPoint;
    }

    public void Respawn()
    {
        //if (championStatus == null)
        //    return;

        //this.championStatus
        //this.championStatus.transform.position = DeployPoint;
    }

    public void RemoveChampion()
    {
        this.championStatus = default; // 기본값으로 초기화하여 제거
        Debug.Log($"Champion removed from tile at {DeployPoint}");
    }
}
