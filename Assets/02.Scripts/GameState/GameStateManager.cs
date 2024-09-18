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
    [Inject] public readonly StagePresenter stagePresenter;

    public StateBehaviour ActiveState => stateMachine.ActiveState;
    //public bool AllowInput => stateMachine.ActiveStateId == playState.StateId || stateMachine.ActiveStateId == pregameState.StateId;
    public bool IsInGame => stateMachine.ActiveState is StageStateBehaviour;

    [Header("Game States Reference")]
    public PregameState pregameState;               // 게임 로비
    public SelectObjectState selectObjectState;     // 순차적으로 중앙에서 기물 선택
    public BattleReadyState battleReadyState;       // 전투전 기물 구매 및 진형 구성
    public BattleState battleState;                 // 전투
    public WinState winState;                       // 게임 승리

    private StateMachine<StateBehaviour> stateMachine;

    [Networked] public TickTimer TransitionTimer { get; set; }
    private float stateDuration;
    public float RemainingTimePercentage => (float)TransitionTimer.RemainingTime(Runner) / stateDuration * 100;

    public bool IsGameStarted { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (TransitionTimer.IsRunning)
        {
            stagePresenter.UpdateProgressBar(RemainingTimePercentage);
        }
    }

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        stateMachine = new StateMachine<StateBehaviour>("Game State", pregameState, selectObjectState, battleReadyState, battleState, winState);
        
        stateMachines.Add(stateMachine);

        foreach (var state in stateMachine.States)
        {
            state.Owner = this;

            // Player가 확인버튼 누르면
            if (state is StageStateBehaviour)
                state.AddTransition(winState, () => !IsGameStarted);
        }

        // Host가 Start버튼 누르면
        pregameState.AddTransition(selectObjectState, () => IsGameStarted);
        // Timer 체크
        selectObjectState.AddTransition(battleReadyState, () => TransitionTimer.Expired(Runner));
        // Timer 체크
        battleReadyState.AddTransition(battleState, () => TransitionTimer.Expired(Runner));
        // Timer 체크
        battleState.AddTransition(battleReadyState, () => TransitionTimer.Expired(Runner));

        //Timer 체크, 스테이지 마지막 전투
        //battleState.AddTransition(selectObjectState, () => TransitionTimer.Expired(Runner) && );
    }

    public void SetTransitionTimer(float delay)
    {
        stateDuration = delay;
        TransitionTimer = TickTimer.CreateFromSeconds(Runner, stateDuration);
    }
}
