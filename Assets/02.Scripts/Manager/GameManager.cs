using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VContainer;

/// <summary>
/// GamePresenter?
/// </summary>
public class GameManager : NetworkBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private NetworkPrefabRef NetworkPlayerPref;

    [Inject] private readonly GameStateManager gameState;

    public Dictionary<PlayerRef, Player> allPlayers { get; private set; } = new Dictionary<PlayerRef, Player>();

    public Player LocalPlayer;

    public Action OnPlayerSpawnedComplete;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GamePlayStart(NetworkRunner runner)
    {
        //this.runner = runner;

        PlayerSpawned(runner);

        gameState.IsGameStarted = true;
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

            PlayerField playerField = playerFields[index++];
            playerField.Object.AssignInputAuthority(player);
            networkObject.GetComponent<Player>().playerField = playerField;

            allPlayers.Add(player, networkObject.GetComponent<Player>());
        }

        PlayerSpawnedComplete();
    }

    private void PlayerSpawnedComplete()
    {
        foreach (var player in allPlayers.Values)
        {
            player.RPC_PlayerInitialize(player.playerField);
        }
    }

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //private void RPC_PlayerInitialize(PlayerRef localPlayerRef)
    //{
    //    LocalPlayer = allPlayers[localPlayerRef];
    //}
}
