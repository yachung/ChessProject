using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomModel : NetworkBehaviour
{
    public int PlayerCount => PlayerDictionary.Count;
    [Networked, Capacity(8), OnChangedRender(nameof(PlayerInfosChanged))]
    private Dictionary<PlayerRef, NetworkPlayerInfo> PlayerDictionary => default; // PlayerRef를 키로 사용하는 딕셔너리로 플레이어 정보 관리

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
    /// 얕은복사로 전달?
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
