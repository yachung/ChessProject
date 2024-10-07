using Fusion;

public class BattleState : StageStateBehaviour
{
    public bool isWin = false;
    public PlayerRef matchingPlayer;

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

    protected override void OnExitState()
    {
        base.OnExitState();
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        if (isWin)
        {
            _stagePresenter.RPC_BattleResult(Runner.LocalPlayer, matchingPlayer);
        }
    }
}
