using Fusion;
using Fusion.Addons.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerField : NetworkBehaviour
{
    [Inject] private readonly GameStateManager gameStateManager;

    [SerializeField] private Transform gridStartingPoint;

    public Tile[,] Tiles = new Tile[9, 10];

    public List<Champion> Champions
    {
        get 
        {
            List<Champion> list = new List<Champion>();

            foreach (var field in Tiles)
            {
                if (gameStateManager.ActiveState is BattleState)
                {
                    if (field.fieldType != FieldType.WaitField)
                        continue;
                }
                else if (gameStateManager.ActiveState is BattleReadyState)
                {
                    if (field.fieldType == FieldType.EnemyField)
                        continue;
                }

                list.Add(field.champion);
            }

            return list;
        }
    }

    public Pose cameraPose;
    public Pose reverseCameraPose;
    public Transform refTransform;
    public Vector3 spawnPosition;

    //private ChampionDragAndDrop championDragHandler;
    //private BattleController battleController;
    private Vector3 gridOffset;
    private BattleController battleController;
    private Vector2 hexSize = new Vector2(13f, 15f);

    private void Awake()
    {
        refTransform = transform;

        cameraPose = new Pose(Camera.main.transform.position + transform.position, Camera.main.transform.rotation);
        reverseCameraPose = new Pose(cameraPose.position + (new Vector3(0, 0, 120)), cameraPose.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));

        gridOffset = gridStartingPoint.position;

        //championDragHandler = GetComponentInChildren<ChampionDragAndDrop>();
        //championDragHandler.playerField = this;
        //
        battleController = GetComponentInChildren<BattleController>();
        battleController.playerField = this;
    }

    private void Start()
    {
        IntializeField();
    }

    private void IntializeField()
    {
        for (int y = 0; y < Tiles.GetLength(1); ++y)
        {
            for (int x = 0; x < Tiles.GetLength(0); ++x)
            {
                FieldType fieldType = FieldType.None;

                if (y == 0 && x >= 0 && x <= 7)
                    fieldType = FieldType.WaitField;
                else if (y >= 1 && y <= 4 && x >= 0 && x <= 7)
                    fieldType = FieldType.BattleField;
                else if (y >= 5 && y <= 9 && x >= 0 && x <= 7)
                    fieldType = FieldType.EnemyField;
                //else if (y == 9 && x >= 0 && x <= 7)
                    //fieldType = FieldType.EnemyField;

                Tiles[x, y] = new Tile(CoordinateToWorldPosition(new Vector2(x, y)), new Vector2Int(x, y), fieldType);
            }
        }
    }

    public void StartBattle()
    {
        // 전투 시작 시 BattleController에서 전투를 시작하도록 호출
        battleController.StartBattle();
    }

    public void SpawnChampion(int x, int y, Champion champion)
    {
        Tiles[x, y].DeployChampion(champion);
    }

    /// <summary>
    /// TestCode
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public List<Tile> GetFields(FieldType fieldType)
    {
        List<Tile> result = new List<Tile>();

        foreach (var fields in Tiles)
        {
            if (fields.fieldType == fieldType)
            {
                result.Add(fields);
            }
        }

        return result;
    }

    public Tile GetEmptyWaitField()
    {
        for (int x = 0; x < 8; x++) // WaitField는 x 좌표가 0부터 7까지 범위에 존재
        {
            if (Tiles[x, 0] != null && !Tiles[x, 0].IsOccupied())
            {
                return Tiles[x, 0];
            }
        }

        // 빈 필드가 없는 경우 null 반환
        return null;
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

        if (Tiles[coordinate.x, coordinate.y].IsOccupied(out placedChampion))
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
            Tiles[origin.x, origin.y].DeployChampion(selectChampion, deployAction);
            return;
        }

        if (Tiles[target.x, target.y].IsOccupied(out Champion placedChampion))
        {
            Tiles[origin.x, origin.y].DeployChampion(placedChampion, deployAction);
            Tiles[target.x, target.y].DeployChampion(selectChampion, deployAction);
        }
        else
        {
            Tiles[target.x, target.y].DeployChampion(selectChampion, deployAction);
            Tiles[origin.x, origin.y].RemoveChampion();
        }
    }

    public void SetChampion(Vector3 origin, Vector3 target, Champion selectChampion, Action<Vector2Int, Vector2Int> deployAction)
    {

        SetChampion(CalculateCoordinate(origin), CalculateCoordinate(target), selectChampion, deployAction);
    }

    public void UpdatePositionOnHost(Vector3 origin, Vector3 target)
    {
        RPC_UpdatePositionOnHost(CalculateCoordinate(origin), CalculateCoordinate(target));
    }    

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_UpdatePositionOnHost(Vector2Int source, Vector2Int target)
    {
        Tiles[source.x, source.y].IsOccupied(out Champion selectedChampion);
        Tiles[target.x, target.y].IsOccupied(out Champion deployedChampion);

        if (selectedChampion == null)
            return;

        if (deployedChampion == null)
        {
            Tiles[target.x, target.y].DeployChampion(selectedChampion);
            Tiles[source.x, source.y].RemoveChampion();
        }
        else
        {
            Tiles[target.x, target.y].DeployChampion(selectedChampion);
            Tiles[source.x, source.y].DeployChampion(deployedChampion);
        }


        //if (FieldArray[source.x, source.y].IsOccupied(out Champion selectedChampion))
        //{
        //    FieldArray[target.x, target.y].DeployChampion(selectedChampion);
        //    //FieldArray[source.x, source.y].RemoveChampion();
        //}

        //if (FieldArray[target.x, target.y].IsOccupied(out Champion deployedChampion))
        //{
        //    FieldArray[source.x, source.y].DeployChampion(deployedChampion);
        //    //FieldArray[target.x, target.y].RemoveChampion();
        //}
        //FieldArray[target.x, target.y].IsOccupied(out Champion deployedChampion);
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
        //return GetHexCoordinate(InputPositionToWorldPosition(inputPosition));
        return GetHexCoordinate(inputPosition);
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
        int approxX = Mathf.RoundToInt((adjustedPosition.x - offsetX) / width);

        // 이 좌표가 정확한 좌표인지, 인접 타일과 비교해 가장 가까운 타일을 찾는 보정 필요
        // 여기서는 간단히 추정 좌표를 반환
        return new Vector2Int(approxX, approxY);
    }

    /// <summary>
    /// 타일 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    private Vector3 CoordinateToWorldPosition(Vector2 coordinate)
    {
        float width = hexSize.x;  // 육각형의 가로 간격
        float height = hexSize.y * 0.75f;  // 세로 간격

        float offsetX = (coordinate.y % 2 == 0) ? 0 : width / 2;  // 짝수 행 오프셋 계산
        float posX = coordinate.x * width + offsetX + gridOffset.x;
        float posY = coordinate.y * height + gridOffset.z;

        return new Vector3(posX, 0, posY);  // 월드 좌표로 변환된 타일 위치
    }
    #endregion
}