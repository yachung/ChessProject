using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class RoomPresenter : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Inject] private readonly SceneLoader sceneLoader;
    private RoomModel roomModel;
    private RoomView roomView;

    [Inject]
    public void Constructor(RoomView roomView, RoomModel roomModel)
    {
        this.roomView = roomView;
        this.roomModel = roomModel;

        roomModel.Initialize(PlayerInfoChangeCallback);

        roomModel.OnIsFindRoomChanged += OnIsFindRoomChanged;
    }

    public void Initialize()
    {
        roomView.Initialize(Runner.IsServer, OnGameStarted);

        UpdateUI();
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
    }

    public void UpdateUI()
    {
        roomView.DisplayPlayerCount(roomModel.PlayerCount);
        roomView.ShowPlayerList(roomModel.GetAllPlayers());
    }

    private void OnIsFindRoomChanged(bool isFindRoom)
    {
        if (isFindRoom)
        {
            roomView.Show();
        }
        else
        {
            roomView.Hide();
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");

        if (runner.IsServer)
        {
            byte[] connectionToken = runner.GetPlayerConnectionToken(player);

            PlayerInfo playerInfo;

            if (connectionToken != null && connectionToken.Length > 0)
            {
                string json = System.Text.Encoding.UTF8.GetString(connectionToken);
                playerInfo = JsonUtility.FromJson<PlayerInfo>(json);
            }
            else
            {
                playerInfo = new PlayerInfo { Name = "Unknown", UserId = "Unknown" };
            }

            //roomModel.AddPlayer(player, new NetworkPlayerInfo(playerInfo));
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            roomModel.RemovePlayer(player);
        }
    }

    public void PlayerInfoChangeCallback()
    {
        UpdateUI();
    }

    public void DeInitialize()
    {
        roomView.gameObject.SetActive(false);
    }

    public void OnGameStarted()
    {
        Debug.Log("GameStart");

        //sceneLoader.LoadScene(SceneType.InGame);
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
