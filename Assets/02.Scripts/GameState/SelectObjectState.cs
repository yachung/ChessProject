using Fusion.Addons.FSM;
using System.Linq;
using VContainer;

public class SelectObjectState : StateBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly SelectField _selectField;


    protected override bool CanEnterState()
    {
        return base.CanEnterState();
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _selectField.SetPlayerPosition(_gameManager.allPlayers.Values.ToArray());
    }

    protected override bool CanExitState(StateBehaviour nextState)
    {
        return base.CanExitState(nextState);
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
