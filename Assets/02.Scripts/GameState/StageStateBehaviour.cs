using Fusion.Addons.FSM;
using UnityEngine;
using VContainer;

public class StageStateBehaviour : StateBehaviour
{
    [Inject] protected readonly StagePresenter _stagePresenter;

    protected override bool CanEnterState()
    {
        bool result = true;

        result &= _stagePresenter.IsTransitionTimerCheck();

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();
        _stagePresenter.Server_SetTransitionTimer(this);
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
        _stagePresenter.StageViewInitialize(this);
        _stagePresenter.OnStageEnter(this);
    }
}
