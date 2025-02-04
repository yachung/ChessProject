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

    private StageStateManager gameState;

    public Action OnPlayerSpawnedComplete;

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

    public override void Spawned()
    {
        Debug.Log("tets");
    }

    public async void GamePlayStart()
    {
        await Runner.LoadScene(SceneRef.FromIndex(2));

        PlayerSpawned(Runner);

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
            //player.playerField = playerField;
            //
            //allPlayers.Add(playerRef, player);
        }
    }

    public void SetGameStateManager(StageStateManager gameStateManager)
    {
        gameState = gameStateManager;
    }
}
