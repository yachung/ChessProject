using Firebase.Auth;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [Networked, Capacity(8)] public NetworkDictionary<PlayerRef, Player> Players => default;

    // 로컬 플레이어 가져오기
    public Player GetLocalPlayer()
    {
        return Players.TryGet(Runner.LocalPlayer, out var player) ? player : null;
    }

    // 특정 플레이어 가져오기
    public Player GetPlayer(PlayerRef playerRef)
    {
        return Players.TryGet(playerRef, out var player) ? player : null;
    }

    // 플레이어 추가
    public void AddPlayer(PlayerRef playerRef, Player player)
    {
        if (!Players.ContainsKey(playerRef))
        {
            Players.Add(playerRef, player);
        }
    }

    // 플레이어 삭제
    public void RemovePlayer(PlayerRef playerRef)
    {
        if (Players.ContainsKey(playerRef))
        {
            Players.Remove(playerRef);
        }
    }

    public List<PlayerRef> PlayerRefList
    {
        get
        {
            List<PlayerRef> list = new List<PlayerRef>();
            foreach (var player in Players)
                list.Add(player.Key);

            return list;
        }
    }

    public List<Player> PlayerList
    {
        get
        {
            List<Player> list = new List<Player>();
            foreach (var player in Players)
                list.Add(player.Value);

            return list;
        }
    }

    public List<PlayerField> PlayerFieldList
    {
        get
        {
            List<PlayerField> list = new List<PlayerField>();
            foreach (var player in Players)
                list.Add(player.Value.playerField);

            return list;
        }
    }
}
