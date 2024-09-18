using Fusion.Addons.FSM;
using System.Linq;
using VContainer;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectObjectState : StageStateBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly SelectField _selectField;

    protected override bool CanEnterState()
    {
        bool result = false;

        switch (Machine.PreviousState)
        {
            case PregameState:
                result = true;
                break;
            case BattleState:
                result = _stagePresenter.IsLastRound();
                break;
        }

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        Debug.Log($"{gameObject.name} is Enter State");
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _selectField.SetPlayerPosition(_gameManager.allPlayers.Values.ToArray());
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
