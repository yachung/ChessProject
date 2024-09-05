using UnityEngine;
using System.Collections.Generic;
using Fusion;
using System;
using TMPro;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_PlayerCount;
    [SerializeField] private Button btn_Start;

    public void InitializeView(bool isHost, Action gameStart)
    {
        gameObject.SetActive(true);

        btn_Start.gameObject.SetActive(isHost);

        if (isHost)
            btn_Start.onClick.AddListener(() => gameStart());
    }

    public void DisplayPlayerCount(int count)
    {
        Debug.Log($"Players: {count}");
    }

    public void ShowPlayerList(Dictionary<PlayerRef, PlayerInfo> playerList)
    {
        foreach (var kvp in playerList)
        {
            Debug.Log($"Player: {kvp.Value.Name}, Ref: {kvp.Key}");
        }
    }
}
