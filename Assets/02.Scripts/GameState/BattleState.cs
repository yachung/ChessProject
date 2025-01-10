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

        foreach (var playerRef in stageModel.matchingPairs.Keys)
        {
            fieldManager.GetFieldByPlayerRef(playerRef).StartBattle();
        }
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
    }

    protected override void OnExitState()
    {
        base.OnExitState();

        foreach (var player in Runner.ActivePlayers)
        {
            fieldManager.GetFieldByPlayerRef(player).BattleEnd();
            fieldManager.MovePlayerToField(player, player);
            fieldManager.GetFieldByPlayerRef(player).ChampionRespawn();
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        if (isWin)
        {
            stageModel.RPC_BattleResult(Runner.LocalPlayer, matchingPlayer);
        }
    }
}
