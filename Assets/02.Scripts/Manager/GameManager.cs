using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkPrefabRef NetworkPlayerPref;

    [Inject] private readonly GameState gameState;

    public Dictionary<PlayerRef, Player> allPlayers { get; private set; } = new Dictionary<PlayerRef, Player>();

    private NetworkRunner runner;


    public void GamePlayStart(NetworkRunner runner)
    {
        this.runner = runner;

        PlayerSpawned(runner);
        gameState.Server_SetState<SelectObjectState>();
    }

    private void PlayerSpawned(NetworkRunner runner)
    {
        if (!runner.IsServer)
            return;

        foreach (var player in runner.ActivePlayers)
        {
            NetworkObject networkObject = runner.Spawn(NetworkPlayerPref, Vector3.zero, Quaternion.identity, player);

            allPlayers.Add(player, networkObject.GetComponent<Player>());
        }
    }

}
