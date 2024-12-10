using UnityEngine;

public static class Utils
{
    public static readonly Vector3Int[] cubeDirections =
        {
            new Vector3Int( 0, 1,-1),
            new Vector3Int( 1, 0,-1),
            new Vector3Int( 1,-1, 0),
            new Vector3Int( 0,-1, 1),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(-1, 1, 0)
        };

    public static Vector3Int Cube_Scale(this Vector3Int hex, int factor)
    {
        return new Vector3Int(hex.x * factor, hex.y * factor, hex.z * factor);
    }

    public static Vector3Int Cube_Add(this Vector3Int hex, Vector3Int vec)
    {
        return hex + vec;
    }

    public static Vector3Int Cube_Neighbor(this Vector3Int hex, int index)
    {
        return Cube_Add(hex, cubeDirections[index]);
    }

    /// <summary>
    /// offset 좌표계를 Cube 좌표계로 변환
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static Vector3Int OffsetToCube(this Vector2Int hex)
    {
        var q = hex.x - (hex.y - (hex.y & 1)) / 2;
        var r = hex.y;

        return new Vector3Int(q, r, -q - r);
    }

    public static Vector2Int CubeToOffset(this Vector3Int hex)
    {
        var x = hex.x + (hex.y - (hex.y & 1)) / 2;
        var y = hex.y;

        return new Vector2Int(x, y);
    }

    public static int GetHexDistance(Vector2Int a, Vector2Int b)
    {
        // Convert a and b from Odd-r offset to cube coordinates.
        Vector3Int distance = OffsetToCube(a) - OffsetToCube(b);

        return Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z));
    }






    public static Vector2Int TransformToOpponentCoordinate(Vector2Int localCoord, int rows, int columns)
    {
        return new Vector2Int(rows - localCoord.x - 1, columns - localCoord.y - 1);
    }

    public static Vector2Int GetHexCoordinate(Vector3 worldPosition, Vector2 gridOffset, Vector2 hexSize)
    {
        float width = hexSize.x;
        float height = hexSize.y * 0.75f;

        Vector3 adjustedPosition = worldPosition - (Vector3)gridOffset;

        int approxY = Mathf.RoundToInt(adjustedPosition.z / height);
        float offsetX = (approxY % 2 == 0) ? 0 : width / 2;
        int approxX = Mathf.RoundToInt((adjustedPosition.x - offsetX) / width);

        return new Vector2Int(approxX, approxY);
    }

    public static Vector3 CoordinateToWorldPosition(Vector2Int coordinate, Vector2 gridOffset, Vector2 hexSize)
    {
        float width = hexSize.x;
        float height = hexSize.y * 0.75f;

        float offsetX = (coordinate.y % 2 == 0) ? 0 : width / 2;
        float posX = coordinate.x * width + offsetX + gridOffset.x;
        float posY = coordinate.y * height + gridOffset.y;

        return new Vector3(posX, 0, posY);
    }
}
