using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;

public class PlayerStateController : NetworkBehaviour, IStateMachineOwner
{
    private PlayerController _playerController;

    private StateMachine<PlayerStateBehaviour> stateMachine;

    private void Awake()
    {
        _playerController = GetComponentInChildren<PlayerController>();
    }

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        var states = GetComponentsInChildren<PlayerStateBehaviour>(true);

        stateMachine = new StateMachine<PlayerStateBehaviour>("Player State", states);

        foreach (var state in states)
        {
            state.Controller = _playerController;
        }

        stateMachines.Add(stateMachine);
    }
}
