using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPresenter : INetworkRunnerCallbacks
{
    private readonly LobbyModel _model;
    private readonly LobbyView _view;

    public LobbyPresenter(LobbyModel model, LobbyView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize(bool isHost)
    {
        // View 초기화 - Presenter에서 View의 초기 설정을 담당
        _view.InitializeView(isHost, OnGameStarted);
    }

    public void UpdateUI()
    {
        _view.DisplayPlayerCount(_model.PlayerCount);
        _view.ShowPlayerList(_model.GetAllPlayers());
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        _model.AddPlayer(player, new PlayerInfo(player.ToString()));
        UpdateUI();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _model.RemovePlayer(player);
        UpdateUI();
    }

    public void OnGameStarted()
    {
        Debug.Log("GameStart");
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
