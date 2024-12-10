using System.Collections.Generic;
using UnityEngine;

public class TileManager
{
    public int TileRows { get; private set; } = 8;
    public int TileColumns { get; private set; } = 10;

    private Tile[,] tiles;
    private Vector3 gridOffset;
    private Vector2 hexSize = new Vector2(13f, 15f);

    public void Initialize(Vector3 offset)
    {
        this.gridOffset = offset;
        tiles = new Tile[TileRows, TileColumns];
    }

    public void InitializeTiles()
    {
        for (int y = 0; y < TileColumns; ++y)
        {
            for (int x = 0; x < TileRows; ++x)
            {
                TileType fieldType = DetermineTileType(x, y);
                tiles[x, y] = new Tile(
                    Utils.CoordinateToWorldPosition(new Vector2Int(x, y), gridOffset, hexSize),
                    new Vector2Int(x, y),
                    fieldType
                );
            }
        }
    }

    private TileType DetermineTileType(int x, int y)
    {
        if (y == 0) return TileType.WaitTile;
        if (y >= 1 && y <= 4) return TileType.BattleTile;
        if (y >= 5) return TileType.EnemyTile;
        return TileType.None;
    }

    public Tile GetTile(Vector2Int coord)
    {
        return IsValidCoordinate(coord) ? tiles[coord.x, coord.y] : null;
    }

    public void ClearAllTiles()
    {
        foreach (var tile in tiles)
        {
            tile?.RemoveChampion();
        }
    }

    public bool IsValidCoordinate(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < TileRows && coord.y >= 0 && coord.y < TileColumns;
    }

    public Vector2Int CalculateCoordinate(Vector3 worldPosition)
    {
        return Utils.GetHexCoordinate(worldPosition, gridOffset, hexSize);
    }

    public void SpawnChampion(Vector2Int coord, Champion champion, bool isOpponentField = false)
    {
        if (isOpponentField)
        {
            coord = Utils.TransformToOpponentCoordinate(coord, TileRows, TileColumns);
        }

        GetTile(coord)?.DeployChampion(champion);
    }
}
