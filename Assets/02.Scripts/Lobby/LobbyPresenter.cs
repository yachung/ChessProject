using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class LobbyPresenter : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Inject] private SceneLoader _sceneLoader;
    [Inject] private LobbyModel _model;
    [Inject] private LobbyView _view;

    //public void Construct(LobbyModel model, LobbyView view, SceneLoader sceneLoader)
    //{
    //    _model = model;
    //    _view = view;
    //    _sceneLoader = sceneLoader;
    //}

    public void Start()
    {
        Debug.Log("Lobby Presenter Initialize");

        _model.Initialize(PlayerInfoChangeCallback);
        _view.btn_Start.onClick.AddListener(() => OnGameStarted());
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);

        _view.Initialize(Runner.IsServer);

        UpdateUI();
    }

    public void UpdateUI()
    {
        _view.DisplayPlayerCount(_model.PlayerCount);
        _view.ShowPlayerList(_model.GetAllPlayers());
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

            _model.AddPlayer(player, playerInfo);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            _model.RemovePlayer(player);
        }
    }

    public void PlayerInfoChangeCallback()
    {
        UpdateUI();
    }

    public void OnGameStarted()
    {
        Debug.Log("GameStart");

        _sceneLoader.Server_OnGameStarted(Runner);
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
