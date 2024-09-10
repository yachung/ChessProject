using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.Addons.FSM;

public class BattleReadyState : StateBehaviour
{
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
    }
}
