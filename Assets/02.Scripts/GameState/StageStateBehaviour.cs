using Fusion;
using Fusion.Addons.FSM;
using VContainer;

public class StageStateBehaviour : StateBehaviour
{
    protected GameManager gameManager => GameManager.Instance;

    [Inject] protected readonly FieldManager fieldManager;

    [Inject] protected readonly ShopPresenter _shopPresenter;
    [Inject] protected readonly ProgressTimer progressTimer;
    [Inject] protected readonly StageModel stageModel;

    private PlayerRef matchedPlayer;

    protected override bool CanEnterState()
    {
        bool result = true;

        result &= progressTimer.IsTransitionTimerCheck();

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();
        progressTimer.SetTransitionTimer(this);
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
        //_stagePresenter.InitializeView(this);
        //_stageModel.OnStageEnter(this);
    }
}
