using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.Addons.FSM;
using VContainer;

public class BattleReadyState : StageStateBehaviour
{
    [Inject] private readonly StageModel _stageModel;

    protected override bool CanEnterState()
    {
        bool result = base.CanEnterState();

        switch (Machine.ActiveState)
        {
            case SelectObjectState:
                result &= true;
                break;
            case BattleState:
                result &= _stagePresenter.IsLastRound() ? false : true;
                break;
        }

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        _stagePresenter.MatchingPlayer();
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _shopPresenter.ShowView();
        _shopPresenter.OnRefreshShop();
    }

    protected override void OnExitState()
    {
        base.OnExitState();

        foreach (var pair in _stageModel.matchingPairs)
        {
            Player Source = _gameManager.allPlayers[pair.Value];
            Player Target = _gameManager.allPlayers[pair.Key];

            Source.MoveToPlayerField(Target.playerField);
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();
    }
}
