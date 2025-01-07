using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class RoomPresenter
{
    private IRoomView roomView;
    private PlayerManager playerManager;

    [Inject]
    public void Constructor(IRoomView roomView, PlayerManager playerManager)
    {
        this.roomView = roomView;
        this.playerManager = playerManager;

        roomModel.Initialize(PlayerInfoChangeCallback);

        roomModel.OnIsFindRoomChanged += OnIsFindRoomChanged;
    }

    public override void Spawned()
    {
        roomView.Initialize(Runner.IsServer, OnGameStarted);
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

        GameManager.Instance.GamePlayStart(roomModel.Runner); 
    }
}
