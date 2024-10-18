using Fusion;
using Fusion.Addons.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class StageModel : NetworkBehaviour
{
    [Inject] private readonly StageDurationConfig stageDurationConfig;

    public int StageIndex { get; set; } = 0;
    public int RoundIndex { get; set; } = 0;
    public string StageName => $"{StageIndex}-{RoundIndex}";

    public Action OnPlayerChanged;

    [Networked, Capacity(8), OnChangedRender("PlayerChanged")] public NetworkDictionary<PlayerRef, Player> PlayerInfos => default;

    public Dictionary<PlayerRef, PlayerRef> matchingPairs = new Dictionary<PlayerRef, PlayerRef>();
    [Networked] public PlayerRef matchingPlayer { get; set; }
    

    public List<PlayerRef> PlayerRefList
    {
        get
        {
            List<PlayerRef> list = new List<PlayerRef>();
            foreach (var player in PlayerInfos)
                list.Add(player.Key);

            return list;
        }
    }

    public List<Player> PlayerList
    {
        get
        {
            List<Player> list = new List<Player>();
            foreach (var player in PlayerInfos)
                list.Add(player.Value);

            return list;
        }
    }

    public List<PlayerField> PlayerFieldList
    {
        get
        {
            List<PlayerField> list = new List<PlayerField>();
            foreach (var player in PlayerInfos)
                list.Add(player.Value.playerField);

            return list;
        }
    }

    [Networked] public TickTimer TransitionTimer { get; set; }

    public float GetStageDuration(StageStateBehaviour state)
    {
        float result = 0f;

        switch (state)
        {
            case SelectObjectState:
                result = stageDurationConfig.selectObjectDuration;
                break;
            case BattleReadyState:
                result = stageDurationConfig.battleReadyDuration;
                break;
            case BattleState:
                result = stageDurationConfig.battleDuration;
                break;
            case WinState:
                result = stageDurationConfig.winDuration;
                break;
        }

        return result;
    }

    private void PlayerChanged()
    {
        OnPlayerChanged?.Invoke();
    }
}
