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
            if (_gameManager.allPlayers.TryGetValue(playerRef, out var player))
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
        foreach (var player in _gameManager.allPlayers.Values)
        {
            player.MoveToPlayerField(player.playerField);
            player.playerField.ChampionRespawn();
        }

        //foreach (var pair in _stageModel.matchingPairs)
        //{
        //    Player Source = _gameManager.allPlayers[pair.Value];
        //    Source.MoveToPlayerField(Source.playerField);
        //    Source.playerField.ChampionRespawn();
        //}
        //foreach (var player in _gameManager.allPlayers.Values)
        //{
        //    player.MoveToPlayerField(player.playerField);
        //}
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
