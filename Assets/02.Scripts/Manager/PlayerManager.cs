using Firebase.Auth;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerManager : NetworkBehaviour
{
    [Inject] private readonly FirebaseManager firebaseManager;

    // 플레이어 참조 -> PlayerInfo 매핑
    private Dictionary<PlayerRef, PlayerInfo> playerInfoDict = new Dictionary<PlayerRef, PlayerInfo>();

    // 플레이어 참조 -> Player 객체 매핑
    private Dictionary<PlayerRef, Player> playerObjectDict = new Dictionary<PlayerRef, Player>();

    public int Count => playerInfoDict.Count;

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            var events = Runner.GetComponent<NetworkEvents>();

            events.PlayerJoined.RemoveListener(OnPlayerJoined);
            events.PlayerJoined.AddListener(OnPlayerJoined);

            events.PlayerLeft.RemoveListener(OnPlayerLeft);
            events.PlayerLeft.AddListener(OnPlayerLeft);
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

            if (!playerInfoDict.ContainsKey(player))
                playerInfoDict.Add(player, playerInfo);
            else
                Debug.Log($"{player}, {playerInfo.Name} 가 이미 목록에 있음.");
        }
    }

    private void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
    {
        // Player 객체 제거
        if (playerObjectDict.TryGetValue(playerRef, out Player player))
        {
            Runner.Despawn(player.Object);
            playerObjectDict.Remove(playerRef);
        }

        // PlayerInfo 제거
        if (playerInfoDict.ContainsKey(playerRef))
        {
            playerInfoDict.Remove(playerRef);
        }
    }

    // 로컬 플레이어 가져오기
    public Player GetLocalPlayer()
    {
        return playerObjectDict.TryGetValue(Runner.LocalPlayer, out var player) ? player : null;
    }

    // 특정 플레이어 가져오기
    public Player GetPlayer(PlayerRef playerRef)
    {
        return playerObjectDict.TryGetValue(playerRef, out var player) ? player : null;
    }

    /// <summary>
    /// OnPlayerJoined와 OnPlayerLeft가 같은 역할을 해주고 있음.
    /// </summary>
    //// 플레이어 추가
    //public void AddPlayer(PlayerRef playerRef, Player player)
    //{
    //    if (!Players.ContainsKey(playerRef))
    //    {
    //        Players.Add(playerRef, player);
    //    }
    //}

    //// 플레이어 삭제
    //public void RemovePlayer(PlayerRef playerRef)
    //{
    //    if (Players.ContainsKey(playerRef))
    //    {
    //        Players.Remove(playerRef);
    //    }
    //}

    public List<PlayerRef> PlayerRefList
    {
        get
        {
            List<PlayerRef> list = new List<PlayerRef>();
            foreach (var player in playerInfoDict)
                list.Add(player.Key);

            return list;
        }
    }

    public List<Player> PlayerList
    {
        get
        {
            List<Player> list = new List<Player>();
            foreach (var player in playerObjectDict)
                list.Add(player.Value);

            return list;
        }
    }
}
