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

        foreach (var playerRef in _stageModel.matchingPairs.Keys)
        {
            if (gameManager.allPlayers.TryGetValue(playerRef, out var player))
            {
                player.playerField.StartBattle();
            }
        }
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();
    }

    protected override void OnExitState()
    {
        base.OnExitState();

        foreach (var player in gameManager.allPlayers.Values)
        {
            player.playerField.BattleEnd();
            player.MoveToPlayerField(player.playerField);
            player.playerField.ChampionRespawn();
        }
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
