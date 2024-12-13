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

    private GameStateManager gameState;

    public Dictionary<PlayerRef, Player> allPlayers { get; private set; } = new Dictionary<PlayerRef, Player>();

    public Player LocalPlayer;

    public Action OnPlayerSpawnedComplete;

    private NetworkRunner runner;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public async void GamePlayStart(NetworkRunner runner)
    {
        this.runner = runner;

        await runner.LoadScene(SceneRef.FromIndex(2));

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

            allPlayers.Add(playerRef, player);

            player.SpawnedComplete();
        }
    }

    public void SetGameStateManager(GameStateManager gameStateManager)
    {
        gameState = gameStateManager;
    }
}
