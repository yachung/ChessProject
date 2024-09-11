using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerField : NetworkBehaviour
{
    [SerializeField] private Transform gridStartingPoint;

    public FieldTile[,] FieldArray = new FieldTile[9, 10];

    public Vector3 cameraPosition;
    public Transform refTransform;
    public Vector3 spawnPosition;

    private Camera mainCamera;
    private Vector3 defaultCameraPosition = new Vector3(3, 45, -55);
    private Vector3 gridOffset;
    private Vector2 hexSize = new Vector2(13f, 15f);

    private void Awake()
    {
        mainCamera = Camera.main;
        refTransform = transform;
        cameraPosition = defaultCameraPosition + transform.position;
        gridOffset = gridStartingPoint.position;
    }

    private void Start()
    {
        IntializeField();
    }

    private void IntializeField()
    {
        for (int y = 0; y < FieldArray.GetLength(1); ++y)
        {
            for (int x = 0; x < FieldArray.GetLength(0); ++x)
            {
                FieldType fieldType = FieldType.None;

                if (y == 0 && x >= 0 && x <= 7)
                    fieldType = FieldType.WaitField;
                else if (y == 9 && x >= 0 && x <= 7)
                    fieldType = FieldType.EnemyField;
                else if (y >= 1 && y <= 8 && x >= 0 && x <= 8)
                    fieldType = FieldType.BattleField;

                FieldArray[x, y] = new FieldTile(CoordinateToWorldPosition(new Vector2(x, y)), fieldType);
            }
        }
    }

    /// <summary>
    /// TestCode
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public List<FieldTile> GetFields(FieldType fieldType)
    {
        List<FieldTile> result = new List<FieldTile>();

        foreach (var fields in FieldArray)
        {
            if (fields.fieldType == fieldType)
            {
                result.Add(fields);
            }
        }

        return result;
    }

    /// <summary>
    /// 커서가 위치한 좌표에 champion이 존재하는지 여부를 리턴하고 존재하면 존재하는 champion을 가져온다.
    /// </summary>
    /// <param name="inputPosition">커서 위치</param>
    /// <param name="placedChampion"></param>
    /// <returns></returns>
    public bool IsOccupied(Vector3 inputPosition, out Champion placedChampion)
    {
        placedChampion = null;

        var coordinate = CalculateCoordinate(inputPosition);

        if (!IsValidCoordinate(coordinate))
            return false;

        Debug.Log(coordinate);

        if (FieldArray[coordinate.x, coordinate.y].IsOccupied(out placedChampion))
            return true;
        else
            return false;
    }

    /// <summary>
    /// target이 유효한 위치가 아니면 selectChampion을 다시 origin 위치로
    /// target에 champion이 있다면 origin 위치로 보내고 selectChampion을 target으로 보냄
    /// target이 비어있다면 selectChampion을 target위치로 보냄.
    /// origin
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="selectChampion"></param>
    public void SetChampion(Vector2Int origin, Vector2Int target, Champion selectChampion, Action<Vector2Int, Vector2Int> deployAction)
    {
        Debug.Log($"origin : {origin}, target : {target}");

        if (!IsValidCoordinate(target))
        {
            FieldArray[origin.x, origin.y].DeployChampion(selectChampion, deployAction);
            return;
        }

        if (FieldArray[target.x, target.y].IsOccupied(out Champion placedChampion))
        {
            FieldArray[origin.x, origin.y].DeployChampion(placedChampion, deployAction);
            FieldArray[target.x, target.y].DeployChampion(selectChampion, deployAction);
        }
        else
        {
            FieldArray[target.x, target.y].DeployChampion(selectChampion, deployAction);
            FieldArray[origin.x, origin.y].RemoveChampion();
        }
    }

    public void SetChampion(Vector3 origin, Vector3 target, Champion selectChampion, Action<Vector2Int, Vector2Int> deployAction)
    {
        SetChampion(CalculateCoordinate(origin), CalculateCoordinate(target), selectChampion, deployAction);
    }

    /// <summary>
    /// 입력된 좌표가 PlayerField에 유효한 좌표인지 확인
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    private bool IsValidCoordinate(Vector2Int coordinate)
    {
        if (coordinate.x >= 0 && coordinate.x <= 8 && coordinate.y >= 0 && coordinate.y <= 9)
            return true;
        else
            return false;
    }

    #region Coordinate Calculate
    public Vector2Int CalculateCoordinate(Vector3 inputPosition)
    {
        return GetHexCoordinate(InputPositionToWorldPosition(inputPosition));
    }

    public Vector3 InputPositionToWorldPosition(Vector3 inputPosition)
    {
        // 그리드가 위치한 평면을 정의 (y=height 평면)
        Plane gridPlane = new Plane(Vector3.up, this.transform.position);

        // 마우스 위치로부터 Ray 생성
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);

        // Ray와 평면의 교차점 계산
        if (gridPlane.Raycast(ray, out float distance))
        {
            // Ray와 평면이 교차하는 지점의 월드 좌표 반환
            return ray.GetPoint(distance);
        }

        return Vector3.zero;  // 실패한 경우 기본값 반환
    }

    private Vector2Int GetHexCoordinate(Vector3 worldPosition)
    {
        // 육각형 타일의 가로 및 세로 간격 계산
        float width = hexSize.x;
        float height = hexSize.y * 0.75f;  // 세로 간격은 육각형이 겹치는 정도를 반영한 값

        Vector3 adjustedPosition = worldPosition - (Vector3)gridOffset;

        // 대략적인 타일 좌표 추정
        int approxY = Mathf.RoundToInt(adjustedPosition.z / height);
        float offsetX = (approxY % 2 == 0) ? 0 : width / 2;  // 짝수/홀수 행에 따른 x 오프셋 계산
        int approxX = Mathf.RoundToInt((adjustedPosition.x + offsetX) / width);

        // 이 좌표가 정확한 좌표인지, 인접 타일과 비교해 가장 가까운 타일을 찾는 보정 필요
        // 여기서는 간단히 추정 좌표를 반환
        return new Vector2Int(approxX, approxY);
    }

    private Vector3 CoordinateToWorldPosition(Vector2 coordinate)
    {
        float width = hexSize.x;  // 육각형의 가로 간격
        float height = hexSize.y * 0.75f;  // 세로 간격

        float offsetX = (coordinate.y % 2 == 0) ? 0 : width / 2;  // 짝수 행 오프셋 계산
        float posX = coordinate.x * width - offsetX + gridOffset.x;
        float posY = coordinate.y * height + gridOffset.z;

        return new Vector3(posX, 0, posY);  // 월드 좌표로 변환된 타일 위치
    }
    #endregion
}