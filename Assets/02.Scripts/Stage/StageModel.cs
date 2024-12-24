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
    [Inject] private readonly PlayerManager playerManager;

    public int StageIndex { get; set; } = 0;
    public int RoundIndex { get; set; } = 0;
    public string StageName => $"{StageIndex}-{RoundIndex}";

    public Dictionary<PlayerRef, PlayerRef> matchingPairs = new Dictionary<PlayerRef, PlayerRef>();
    [Networked] public PlayerRef matchingPlayer { get; set; }

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
}
