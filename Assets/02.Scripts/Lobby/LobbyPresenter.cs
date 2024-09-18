using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class LobbyPresenter : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly LobbyModel _lobbyModel;
    [Inject] private readonly LobbyView _lobbyView;


    public void Start()
    {
        Debug.Log("Lobby Presenter Initialize");

        _lobbyModel.Initialize(PlayerInfoChangeCallback);
        _lobbyView.btn_Start.onClick.AddListener(() => OnGameStarted());
    }

    public void Initialize()
    {
        _lobbyView.Initialize(Runner.IsServer);

        UpdateUI();
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
    }

    public void UpdateUI()
    {
        _lobbyView.DisplayPlayerCount(_lobbyModel.PlayerCount);
        _lobbyView.ShowPlayerList(_lobbyModel.GetAllPlayers());
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");

        if (runner.IsServer)
        {
            PlayerInfo playerInfo = new PlayerInfo
            {
                Index = runner.SessionInfo.PlayerCount,
                Name = player.ToString()
            };

            _lobbyModel.AddPlayer(player, playerInfo);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            _lobbyModel.RemovePlayer(player);
        }
    }

    public void PlayerInfoChangeCallback()
    {
        UpdateUI();
    }

    public void DeInitialize()
    {
        _lobbyView.gameObject.SetActive(false);
    }

    public void OnGameStarted()
    {
        Debug.Log("GameStart");

        _gameManager.GamePlayStart(Runner);
    }

    #region NotUseCallBack
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    #endregion
}
