using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class LobbyModel
{
    public int PlayerCount => _playerDictionary.Count;
    private Dictionary<PlayerRef, PlayerInfo> _playerDictionary; // PlayerRef�� Ű�� ����ϴ� ��ųʸ��� �÷��̾� ���� ����

    public LobbyModel()
    {
        _playerDictionary = new Dictionary<PlayerRef, PlayerInfo>();
    }

    public void AddPlayer(PlayerRef playerRef, PlayerInfo playerInfo)
    {
        if (!_playerDictionary.ContainsKey(playerRef))
        {
            _playerDictionary.Add(playerRef, playerInfo);
        }
        else
        {
            Debug.LogWarning($"{playerRef} is already exist!!!!!!!!");
        }
    }

    public void RemovePlayer(PlayerRef playerRef)
    {
        if (_playerDictionary.ContainsKey(playerRef))
        {
            _playerDictionary.Remove(playerRef);
        }
    }

    public PlayerInfo GetPlayerInfo(PlayerRef playerRef)
    {
        return _playerDictionary.ContainsKey(playerRef) ? _playerDictionary[playerRef] : default;
    }

    /// <summary>
    /// ��������� ����?
    /// </summary>
    /// <returns></returns>
    public Dictionary<PlayerRef, PlayerInfo> GetAllPlayers()
    {
        return new Dictionary<PlayerRef, PlayerInfo>(_playerDictionary);
    }
}
