using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the main state of the game and contains the networked FSM.
/// </summary>
public class GameState : NetworkBehaviour, IStateMachineOwner
{
    public StateBehaviour ActiveState => stateMachine.ActiveState;
    //public bool AllowInput => stateMachine.ActiveStateId == playState.StateId || stateMachine.ActiveStateId == pregameState.StateId;
    //public bool IsInGame => stateMachine.ActiveStateId == battleReadyState.StateId || stateMachine.ActiveStateId == battleState.StateId || stateMachine.ActiveStateId == selectObjectState.StateId;

    [Networked] TickTimer Delay { get; set; }
    [Networked] int DelayedStateId { get; set; }

    [Header("Game States Reference")]
    //public StateBehaviour[] States;
    public PregameState pregameState;               // 게임 로비
    public BattleReadyState battleReadyState;       // 전투전 기물 구매 및 진형 구성
    public BattleState battleState;                 // 전투
    public SelectObjectState selectObjectState;     // 순차적으로 중앙에서 기물 선택
    public WinState winState;                       // 게임 승리

    private StateMachine<StateBehaviour> stateMachine;

    public override void Spawned()
    {
        Debug.Log($"{name} is Spawned");
    }

    public override void FixedUpdateNetwork()
    {
        if (DelayedStateId >= 0 && Delay.ExpiredOrNotRunning(Runner))
        {
            stateMachine.ForceActivateState(DelayedStateId);
            DelayedStateId = -1;
        }
    }

    public void Server_SetState<T>() where T : StateBehaviour
    {
        Assert.Check(HasStateAuthority, "Clients cannot set GameState");

        Delay = TickTimer.None;
        DelayedStateId = stateMachine.GetState<T>().StateId;
    }

    public void Server_DelaySetState<T>(float delay) where T : StateBehaviour
    {
        Assert.Check(HasStateAuthority, "Clients cannot set GameState");

        Debug.Log($"Delay state change to {nameof(T)} for {delay}s");
        Delay = TickTimer.CreateFromSeconds(Runner, delay);
        DelayedStateId = stateMachine.GetState<T>().StateId;
    }

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        stateMachine = new StateMachine<StateBehaviour>("Game State", pregameState, battleReadyState, battleState, selectObjectState, winState);
        stateMachines.Add(stateMachine);
    }
}
