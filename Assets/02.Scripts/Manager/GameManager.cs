using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// GamePresenter?
/// </summary>
public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef NetworkPlayerPref;

    [Inject] private readonly GameView gameView;
    [Inject] private readonly GameStateManager gameState;

    public Dictionary<PlayerRef, Player> allPlayers { get; private set; } = new Dictionary<PlayerRef, Player>();

    private NetworkRunner runner;


    public void GamePlayStart(NetworkRunner runner)
    {
        this.runner = runner;

        PlayerSpawned(runner);

        gameState.IsGameStarted = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (gameState.TransitionTimer.IsRunning)
        {
            //float remainTime = (float)gameState.TransitionTimer.RemainingTime(Runner);
            //Debug.Log($"{gameState.ActiveState.Name} RemainTime : {remainTime}");
            ////float currentPercent = (remainTime / gameState.ActiveState.StateDuration) * 100;
            gameView.progressBar.SetValue(gameState.RemainingTimePercentage);
        }
    }

    private void PlayerSpawned(NetworkRunner runner)
    {
        if (!runner.IsServer)
            return;

        PlayerField[] playerFields = FindObjectsByType<PlayerField>(FindObjectsSortMode.None);

        int index = 0;

        foreach (var player in runner.ActivePlayers)
        {
            NetworkObject networkObject = runner.Spawn(NetworkPlayerPref, Vector3.zero, Quaternion.identity, player);
            networkObject.GetComponent<Player>().RPC_PlayerFieldInitialize(playerFields[index]);
            runner.SetPlayerObject(player, networkObject);

            allPlayers.Add(player, networkObject.GetComponent<Player>());
        }
    }
}
