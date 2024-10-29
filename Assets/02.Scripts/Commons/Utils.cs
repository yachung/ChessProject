using UnityEngine;

public static class Utils
{
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
}
