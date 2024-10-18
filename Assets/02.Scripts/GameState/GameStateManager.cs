using Fusion;
using Fusion.Addons.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// Handles the main state of the game and contains the networked FSM.
/// </summary>
public class GameStateManager : NetworkBehaviour, IStateMachineOwner
{
    public StateBehaviour ActiveState => stateMachine.ActiveState;
    //public bool AllowInput => stateMachine.ActiveStateId == playState.StateId || stateMachine.ActiveStateId == pregameState.StateId;
    public bool IsInGame => stateMachine.ActiveState is StageStateBehaviour;
    public bool IsBattle => stateMachine.ActiveState is BattleState;

    [Header("Game States Reference")]
    public PregameState pregameState;               // 게임 로비
    public SelectObjectState selectObjectState;     // 순차적으로 중앙에서 기물 선택
    public BattleReadyState battleReadyState;       // 전투전 기물 구매 및 진형 구성
    public BattleState battleState;                 // 전투
    public WinState winState;                       // 게임 승리

    private StateMachine<StateBehaviour> stateMachine;

    public bool IsGameStarted { get; set; }

    /// <summary>
    /// Proxy에서 허용되지 않음
    /// </summary>
    //public override void FixedUpdateNetwork()
    //{
    //    RemainingTimePercentage = (float)TransitionTimer.RemainingTime(Runner) / stateDuration * 100;
    //}

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        stateMachine = new StateMachine<StateBehaviour>("Game State", pregameState, selectObjectState, battleReadyState, battleState, winState);
        
        stateMachines.Add(stateMachine);

        foreach (var state in stateMachine.States)
        {
            state.Owner = this;

            // 승리 조건
            if (state is StageStateBehaviour && state is not WinState)
            {
                //state.AddTransition(winState, () => !IsGameStarted);
            }
        }

        // Host가 Start버튼 누르면
        pregameState.AddTransition(selectObjectState,       () => IsGameStarted);

        selectObjectState.AddTransition(battleReadyState,   () => true);
        battleReadyState.AddTransition(battleState,         () => true);
        battleState.AddTransition(battleReadyState,         () => true);
        battleState.AddTransition(selectObjectState,        () => true);
        winState.AddTransition(pregameState,                () => !IsGameStarted);

        //Timer 체크, 스테이지 마지막 전투
        //battleState.AddTransition(selectObjectState, () => TransitionTimer.Expired(Runner) && );
    }
}
