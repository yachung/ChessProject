using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.Addons.FSM;
using VContainer;

public class BattleReadyState : StageStateBehaviour
{
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
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _shopPresenter.ShowUI();
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();
    }
}
