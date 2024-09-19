using Fusion;
using Fusion.Addons.FSM;
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

    [Networked] public TickTimer TransitionTimer { get; set; }

    //public float RemainingTimePercentage => (float)TransitionTimer.RemainingTime(Runner) / stateDuration * 100;

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
