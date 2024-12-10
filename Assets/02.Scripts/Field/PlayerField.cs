using Fusion;
using System;
using UnityEngine;
using VContainer;

public class PlayerField : NetworkBehaviour
{
    [Inject] private TileManager tileManager;
    [Inject] private ChampionManager championManager;
    [Inject] private BattleManager battleManager;

    [Networked, OnChangedRender(nameof(IsBattleChanged))] public bool IsBattle { get; set; }
    public Action<bool> OnIsBattleChanged;

    [SerializeField] private Transform gridStartingPoint;

    private void Awake()
    {
        tileManager.Initialize(gridStartingPoint.position);
        championManager.Initialize(tileManager, this);
        battleManager.Initialize(this, tileManager, championManager);
    }

    private void Start()
    {
        tileManager.InitializeTiles();
    }

    /// <summary>
    /// 전투 상태 변경 시 호출되는 메서드
    /// </summary>
    public void IsBattleChanged()
    {
        OnIsBattleChanged?.Invoke(IsBattle);
        battleManager.HandleBattleStateChange(IsBattle);
    }
}
