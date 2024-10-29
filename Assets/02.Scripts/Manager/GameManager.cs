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

    [Inject] private readonly IObjectResolver container; // VContainer의 DI 컨테이너
    [Inject] private readonly GameStateManager gameState;
    [Inject] private readonly StageModel stageModel;

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
        foreach (var playerRef in runner.ActivePlayers)
        {
            NetworkObject networkObject = runner.Spawn(NetworkPlayerPref, Vector3.zero, Quaternion.identity, playerRef);

            PlayerField playerField = playerFields[index++];
            playerField.Object.AssignInputAuthority(playerRef);

            Player player = networkObject.GetComponent<Player>();
            player.playerField = playerField;
            container.Inject(player);

            stageModel.PlayerInfos.Add(playerRef, player);
            allPlayers.Add(playerRef, player);

            player.SpawnedComplete();
        }
    }

    //private void PlayerSpawnedComplete()
    //{
    //    foreach (var player in allPlayers.Values)
    //    {
    //        player.RPC_PlayerInitialize(player.playerField);
    //    }
    //}

    //public void Set

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //private void RPC_PlayerInitialize(PlayerRef localPlayerRef)
    //{
    //    LocalPlayer = allPlayers[localPlayerRef];
    //}
}
