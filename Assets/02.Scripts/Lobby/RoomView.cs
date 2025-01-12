using UnityEngine;
using System.Collections.Generic;
using Fusion;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using VContainer;

public interface IRoomView : IView
{
    void Initialize(bool isServer, Action gameStart);
    void DisplayPlayerCount(int count);
    void ShowPlayerList(List<NetworkPlayerInfo> playerList);
}

public class RoomView : MonoBehaviour, IRoomView
{
    [Inject] private readonly UIManager uiManager;

    [SerializeField] private TMP_Text txt_PlayerCount;
    [SerializeField] private Button btn_Start;
    [SerializeField] private PlayerInfoCell[] playerInfoCells;

    private void Awake()
    {
        foreach (var cell in playerInfoCells)
            cell.gameObject.SetActive(false);
    }

    public void Initialize(bool isHost, Action gameStart)
    {
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

    public void ShowPlayerList(List<NetworkPlayerInfo> playerList)
    {
        int index = 0;

        foreach (var cell in playerInfoCells)
        {
            if (index < playerList.Count)
            {
                // playerList.Values에서 플레이어 정보를 가져와 초기화
                cell.gameObject.SetActive(true);  // 셀 활성화
                cell.Initialize(playerList[index]);  // 해당 인덱스의 플레이어 정보로 초기화
                index++;
            }
            else
            {
                // 남은 셀들은 비활성화
                cell.gameObject.SetActive(false);
            }
        }
    }

    public void RefreshUI()
    {

    }

    public void ShowLoading(bool result)
    {
        uiManager.ShowLoading(result);
    }

    public void ShowMessage(string message)
    {
        uiManager.ShowMessage(message);
    }
}
