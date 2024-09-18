using Fusion.Addons.FSM;
using UnityEngine;
using VContainer;

public class StageStateBehaviour : StateBehaviour
{
    [Inject] protected readonly StagePresenter _stagePresenter;
    [SerializeField] private float stateDuration;
    public float StateDuration => stateDuration;

    protected override void OnEnterState()
    {
        base.OnEnterState();

        Owner.SetTransitionTimer(StateDuration);
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
        _stagePresenter.StageViewInitialize(this);
    }
}
