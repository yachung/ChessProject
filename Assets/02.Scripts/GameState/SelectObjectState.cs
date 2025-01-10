using Fusion.Addons.FSM;
using System.Linq;
using VContainer;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Fusion;

public class SelectObjectState : StageStateBehaviour
{
    protected override bool CanEnterState()
    {
        bool result = base.CanEnterState();

        switch (Machine.ActiveState)
        {
            //case PregameState:
            //    result &= true;
            //    break;
            case BattleState:
                result &= stageModel.IsLastRound();
                break;
        }

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        Debug.Log($"{gameObject.name} is Enter State");

        _shopPresenter.HideView();

        fieldManager.MoveAllPlayersToSelectField();
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
    }

    protected override void OnExitState()
    {
        base.OnExitState();

        foreach (var playerRef in Runner.ActivePlayers)
        {
            fieldManager.MovePlayerToField(playerRef, playerRef);
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();
        //Player LocalPlayer = _gameManager.LocalPlayer;
        //LocalPlayer.SetPlayerCamera(LocalPlayer.playerField.cameraPose);
    }
}
