using Fusion;
using Fusion.Addons.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerField : NetworkBehaviour
{
    private readonly Vector2 hexSize = new Vector2(13f, 15f);

    [Inject] private readonly GameStateManager gameStateManager;
    private BattleController battleController;

    [SerializeField] private Transform gridStartingPoint;
    private Tile[,] Tiles = new Tile[8, 10];
    private Tile this[Vector2Int coord, bool isDrag = false]
    {
        get
        {
            if (isDrag)
            {
                if (!IsBattle && (coord.x is >= 0 and <= 7) && (coord.y is >= 0 and <= 4))
                    return Tiles[coord.x, coord.y];

                if (IsBattle && (coord.x is >= 0 and <= 7) && (coord.y == 0))
                    return Tiles[coord.x, coord.y];

                return null;
            }

            if (IsBattle && (coord.x is >= 0 and <= 7) && (coord.y is >= 1 and <= 8))
                return Tiles[coord.x, coord.y];

            if (!IsBattle && (coord.x is >= 0 and <= 7) && (coord.y is >= 0 and <= 9))
                return Tiles[coord.x, coord.y];

            return null;
        }
    }
    private Vector3 tileGridOffset;

    [Networked, OnChangedRender(nameof(IsBattleChanged))] public bool IsBattle { get; set; }
    [Networked, Capacity(40)] public NetworkLinkedList<Champion> Champions => default;
    public Action<bool> OnIsBattleChanged;

    public Pose cameraPose;
    public Pose reverseCameraPose;


    #region 초기화 및 설정

    private void Awake()
    {
        cameraPose = new Pose(Camera.main.transform.position + transform.position, Camera.main.transform.rotation);
        reverseCameraPose = new Pose(cameraPose.position + (new Vector3(0, 0, 120)), Quaternion.Euler(0, 180, 0) * cameraPose.rotation);

        tileGridOffset = gridStartingPoint.position;

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
                TileType fieldType = TileType.None;

                if (y == 0 && x >= 0 && x <= 7)
                    fieldType = TileType.WaitTile;
                else if (y >= 1 && y <= 4 && x >= 0 && x <= 7)
                    fieldType = TileType.BattleTile;
                else if (y >= 5 && y <= 9 && x >= 0 && x <= 7)
                    fieldType = TileType.EnemyTile;

                Tiles[x, y] = new Tile(CoordinateToWorldPosition(new Vector2(x, y)), new Vector2Int(x, y), fieldType);
            }
        }
    }

    #endregion

    #region 타일 관련 메서드

    /// <summary>
    /// 전투 타일 반환
    /// </summary>
    public Tile GetBattleTile(Vector2Int coord) => this[coord];

    /// <summary>
    /// 드래그 가능한 타일 반환
    /// </summary>
    public Tile GetDragTile(Vector2Int coord) => this[coord, isDrag: true];

    /// <summary>
    /// TestCode
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public List<Tile> GetTiles(TileType fieldType)
    {
        List<Tile> result = new List<Tile>();

        foreach (var fields in Tiles)
        {
            if (fields.tileType == fieldType)
            {
                result.Add(fields);
            }
        }

        return result;
    }

    /// <summary>
    /// 대기 필드에서 빈 타일 반환
    /// </summary>
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

    #endregion

    #region 챔피언 배치 및 관리


    /// <summary>
    /// 챔피언을 타일에 배치 및 이동
    /// </summary>
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

    /// <summary>
    /// 챔피언 생성
    /// </summary>
    public void SpawnChampion(Vector2Int Coord, Champion champion)
    {
        this[Coord].DeployChampion(champion);
    }

    /// <summary>
    /// 내가 보유한 챔피언들 내 필드의 ReadyCoord로 재소환
    /// </summary>
    public void ChampionRespawn()
    {
        foreach (var champion in Champions)
        {
            champion.Respawn();
            SpawnChampion(champion.ReadyCoord, champion);
        }
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

        if (GetDragTile(coordinate) == null)
            return false;

        Debug.Log(coordinate);

        if (GetDragTile(coordinate).IsOccupied(out var champion))
        {
            placedChampion = champion;
            return true;
        }
        else
            return false;
    }

    public void UpdatePositionOnHost(Vector3 origin, Vector3 target)
    {
        RPC_UpdatePositionOnHost(CalculateCoordinate(origin), CalculateCoordinate(target));
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_UpdatePositionOnHost(Vector2Int source, Vector2Int target)
    {
        if (GetDragTile(target) == null)
        {
            this[source].RespawnChampion();

            return;
        }

        this[source].IsOccupied(out Champion selectedChampion);
        this[target].IsOccupied(out Champion deployedChampion);

        if (selectedChampion == null)
            return;

        if (deployedChampion == null)
        {
            this[target].DeployChampion(selectedChampion);
            this[source].RemoveChampion();
        }
        else
        {
            this[target].DeployChampion(selectedChampion);
            this[source].DeployChampion(deployedChampion);
        }
    }

    #endregion

    #region 전투 시작 및 종료
    public void IsBattleChanged()
    {
        OnIsBattleChanged?.Invoke(IsBattle);
    }

    /// <summary>
    /// 전투 시작 시 초기화 작업
    /// </summary>
    public void StartBattle()
    {
        IsBattle = true;

        // 전투 시작 시 BattleController에서 전투를 시작하도록 호출
        battleController.StartBattle();
    }

    public void BattleInitializeForEnemy(List<Tile> championTiles)
    {
        foreach (var tile in championTiles)
        {
            Vector2Int target = new Vector2Int(Tiles.GetLength(0) - tile.Coordinate.x - 1, Tiles.GetLength(1) - tile.Coordinate.y - 1);

            if (tile.Champion != null)
            {
                tile.Champion.IsAwayTeam = true;
                this[target].DeployChampion(tile.Champion, true);
            }
        }
    }

    /// <summary>
    /// 전투 종료시 초기화 작업
    /// </summary>
    public void BattleEnd()
    {
        IsBattle = false;

        foreach (var tile in Tiles)
            tile.RemoveChampion();

        battleController.BattleEnd();

        foreach (var champion in Champions)
            champion.IsAwayTeam = false;
    }

    #endregion

    #region 좌표 계산

    public Vector2Int CalculateCoordinate(Vector3 inputPosition)
    {
        return GetHexCoordinate(inputPosition);
    }

    private Vector2Int GetHexCoordinate(Vector3 worldPosition)
    {
        // 육각형 타일의 가로 및 세로 간격 계산
        float width = hexSize.x;
        float height = hexSize.y * 0.75f;  // 세로 간격은 육각형이 겹치는 정도를 반영한 값

        Vector3 adjustedPosition = worldPosition - (Vector3)tileGridOffset;

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
        float posX = coordinate.x * width + offsetX + tileGridOffset.x;
        float posY = coordinate.y * height + tileGridOffset.z;

        return new Vector3(posX, 0, posY);  // 월드 좌표로 변환된 타일 위치
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

    #endregion
}