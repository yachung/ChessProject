using Fusion.Addons.FSM;
using System.Linq;
using VContainer;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Fusion;

public class SelectObjectState : StageStateBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly SelectField _selectField;

    protected override bool CanEnterState()
    {
        bool result = base.CanEnterState();

        switch (Machine.ActiveState)
        {
            case PregameState:
                result &= true;
                break;
            case BattleState:
                result &= _stagePresenter.IsLastRound();
                break;
        }

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        Debug.Log($"{gameObject.name} is Enter State");

        _shopPresenter.HideView();
        _selectField.SetPlayerPosition(_gameManager.allPlayers.Values.ToArray());
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
    }

    protected override void OnExitState()
    {
        base.OnExitState();
        foreach (var player in _gameManager.allPlayers.Values)
        {
            player.MoveToPlayerField(player.playerField);
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();
    }
}
