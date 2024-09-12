using Fusion.Addons.FSM;
using System.Linq;
using VContainer;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectObjectState : StateBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly SelectField _selectField;

    private float stateDuration;

    protected override bool CanEnterState()
    {
        return base.CanEnterState();
    }

    protected override void OnEnterState()
    {
        Debug.Log($"{gameObject.name} is Enter STate");

        UniTask.WaitForSeconds(100);

        base.OnEnterState();
    }

    //private async UniTask ChangeState()
    //{
    //    await UniTask.WaitForSeconds(100);
    //    Machine.
    //}

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _selectField.SetPlayerPosition(_gameManager.allPlayers.Values.ToArray());
    }

    protected override bool CanExitState(StateBehaviour nextState)
    {
        return Machine.StateTime > stateDuration;
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
