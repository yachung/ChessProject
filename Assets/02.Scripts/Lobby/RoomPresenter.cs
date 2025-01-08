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
    }

    public void UpdateUI()
    {
        roomView.DisplayPlayerCount(playerManager.Players.Count);
        roomView.ShowPlayerList(playerManager.Players);
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

    public void OnGameStarted()
    {
        Debug.Log("GameStart");

        GameManager.Instance.GamePlayStart(roomModel.Runner); 
    }
}
