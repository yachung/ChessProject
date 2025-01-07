using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용하지 않음. PlayerManager가 RoomModel의 역할을 대체
/// </summary>
/// 
public class RoomModel : NetworkBehaviour
{
    public int PlayerCount => PlayerDictionary.Count;
    [Networked, Capacity(8), OnChangedRender(nameof(PlayerInfosChanged))]
    private NetworkDictionary<PlayerRef, NetworkPlayerInfo> PlayerDictionary => default; // PlayerRef를 키로 사용하는 딕셔너리로 플레이어 정보 관리

    private Action OnPlayerInfoChangedRender;
    public Action<bool> OnIsFindRoomChanged;

    private bool _isFindRoom;

    public bool IsFindRoom
    {
        get => _isFindRoom;
        set
        {
            if (_isFindRoom != value)
            {
                _isFindRoom = value;
                OnIsFindRoomChanged?.Invoke(_isFindRoom);
            }
        }
    }

    public void Initialize(Action action)
    {
        OnPlayerInfoChangedRender = action;
    }

    public override void Spawned()
    {
        if (Runner.LocalPlayer == Object.InputAuthority)
        {
            var events = Runner.GetComponent<NetworkEvents>();

            events.PlayerJoined.RemoveListener(OnPlayerJoined);
            events.PlayerJoined.AddListener(OnPlayerJoined);

            events.PlayerLeft.RemoveListener(OnPlayerLeft);
            events.PlayerLeft.AddListener(OnPlayerLeft);

        }

        PlayerInfosChanged();
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

            NetworkPlayerInfo networkPlayerInfo = new NetworkPlayerInfo();

            networkPlayerInfo.Index = playerInfo.Index;
            networkPlayerInfo.Name = playerInfo.Name;
            networkPlayerInfo.UserId = playerInfo.UserId;

            AddPlayer(player, networkPlayerInfo);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            RemovePlayer(player);
        }
    }

    public void AddPlayer(PlayerRef playerRef, NetworkPlayerInfo playerInfo)
    {
        if (!PlayerDictionary.ContainsKey(playerRef))
        {
            PlayerDictionary.Add(playerRef, playerInfo);
        }
        else
        {
            Debug.LogWarning($"{playerRef} is already exist!!!!!!!!");
        }
    }

    public void RemovePlayer(PlayerRef playerRef)
    {
        if (PlayerDictionary.ContainsKey(playerRef))
        {
            PlayerDictionary.Remove(playerRef);
        }
    }

    public NetworkPlayerInfo GetPlayerInfo(PlayerRef playerRef)
    {
        return PlayerDictionary.ContainsKey(playerRef) ? PlayerDictionary[playerRef] : default;
    }

    /// <summary>
    /// 얕은복사로 전달
    /// </summary>
    /// <returns></returns>
    public Dictionary<PlayerRef, NetworkPlayerInfo> GetAllPlayers()
    {
        return new Dictionary<PlayerRef, NetworkPlayerInfo>(PlayerDictionary);
    }

    public void PlayerInfosChanged()
    {
        OnPlayerInfoChangedRender?.Invoke();
    }
}
