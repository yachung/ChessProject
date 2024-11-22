using UnityEngine;
using System.Collections.Generic;
using Fusion;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class RoomView : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_PlayerCount;
    [SerializeField] private Button btn_Start;
    [SerializeField] private PlayerInfoCell[] playerInfoCells;

    public void Initialize(bool isHost, Action gameStart)
    {
        foreach (var cell in playerInfoCells)
            cell.gameObject.SetActive(false);

        gameObject.SetActive(true);
        btn_Start.gameObject.SetActive(isHost);
        btn_Start.onClick.AddListener(() => gameStart());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayPlayerCount(int count)
    {
        txt_PlayerCount.text = count.ToString();
    }

    public void ShowPlayerList(Dictionary<PlayerRef, NetworkPlayerInfo> playerList)
    {
        int index = 0;

        foreach (var cell in playerInfoCells)
        {
            if (index < playerList.Count)
            {
                // playerList.Values에서 플레이어 정보를 가져와 초기화
                cell.gameObject.SetActive(true);  // 셀 활성화
                cell.Initialize(playerList.Values.ElementAt(index));  // 해당 인덱스의 플레이어 정보로 초기화
                index++;
            }
            else
            {
                // 남은 셀들은 비활성화
                cell.gameObject.SetActive(false);
            }
        }
    }
}
