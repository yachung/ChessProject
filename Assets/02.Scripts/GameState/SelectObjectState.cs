using Fusion.Addons.FSM;
using System.Linq;
using VContainer;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectObjectState : StateBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly SelectField _selectField;

    [SerializeField] private float stateDuration;

    protected override bool CanEnterState()
    {
        return base.CanEnterState();
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        Debug.Log($"{gameObject.name} is Enter State");

        StateManager.SetTransitionTimer(stateDuration);
        //StateManager.Server_DelaySetState<BattleReadyState>(stateDuration);
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
        //StateManager.Server_DelaySetState<BattleReadyState>(5f);

        _selectField.SetPlayerPosition(_gameManager.allPlayers.Values.ToArray());
    }

    protected override bool CanExitState(StateBehaviour nextState)
    {
        //return Machine.StateTime > stateDuration;
        return true;
    }

    protected override void OnExitState()
    {
        base.OnExitState();
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        foreach (var player in _gameManager.allPlayers.Values)
        {
            player.MoveToPlayerField();
        }
    }
}
