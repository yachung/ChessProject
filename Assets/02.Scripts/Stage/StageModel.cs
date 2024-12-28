using Fusion;
using Fusion.Addons.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class StageModel : NetworkBehaviour
{
    [Inject] private readonly ProgressTimer progressTimer;
    [Inject] private readonly PlayerManager playerManager;

    public int StageIndex { get; set; } = 0;
    public int RoundIndex { get; set; } = 0;
    public string StageName => $"{StageIndex}-{RoundIndex}";

    public float StageProgress { get; private set; }

    private StageStateBehaviour ActiveStageState;


    public Dictionary<PlayerRef, PlayerRef> matchingPairs = new Dictionary<PlayerRef, PlayerRef>();
    [Networked] public PlayerRef matchingPlayer { get; set; }

    public void OnStageEnter(StageStateBehaviour stageState)
    {
        ActiveStageState = stageState;

        switch (stageState)
        {
            case BattleReadyState:
                if (IsLastRound())
                {
                    StageIndex++;
                    RoundIndex = 1;
                }
                else
                {
                    RoundIndex++;
                }
                break;
        }
    }

    public bool IsLastRound()
    {
        return RoundIndex > 1;
    }
}
