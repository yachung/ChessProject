using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// GamePresenter?
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkPrefabRef NetworkPlayerPref;

    [Inject] private readonly GameStateManager gameState;

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

    /// <summary>
    /// Only Test Method
    /// Next State
    /// </summary>
    private void OnGUI()
    {
        //if (GUI.Button(new Rect(Screen.width, Screen.height, 200, 40), "Next State"))
        //    StartGame(GameMode.AutoHostOrClient);
    }
}
