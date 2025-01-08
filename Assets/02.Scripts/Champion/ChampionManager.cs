using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// ChampionManager는 모든 챔피언을 관리하고, 플레이어별 챔피언 목록을 별도로 유지한다.
/// </summary>
public class ChampionManager : NetworkBehaviour
{
    // 모든 챔피언을 관리하는 네트워크 딕셔너리
    [Networked, Capacity(200)] private NetworkDictionary<int, ChampionData> championDataDict => default;

    // 서버 측에서만 관리하는 플레이어별 챔피언 목록
    private Dictionary<PlayerRef, List<int>> playerChampions = new Dictionary<PlayerRef, List<int>>();

    // 클라이언트가 소환 요청을 보내면 서버에서 소환을 처리
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SummonChampion(NetworkString<_32> name, PlayerRef player)
    {
        if (Runner.IsServer)
        {
            SummonChampion(name.Value, player);
        }
    }

    private void SummonChampion(string name, PlayerRef playerRef)
    {
        if (GameManager.Instance.allPlayers.TryGetValue(playerRef, out Player player))
        {
            ChampionData championData = Resources.Load($"Data/{name}Data") as ChampionData;

            player.Gold -= championData.cost;

            NetworkPrefabRef championPrefab = championData.championPrefab;
            
            Vector3 spawnPosition;

            Tile emptyTile = player.playerField.GetEmptyWaitField();

            if (emptyTile == null)
            {
                Debug.LogWarning("Failed to spawn a champion.");
                return;
            }
            else
                spawnPosition = emptyTile.DeployPoint;

            if (Runner.Spawn(championPrefab, spawnPosition, Quaternion.identity, playerRef).TryGetComponent<Champion>(out var champion))
            {
                champion.RPC_DataInitialize(new ChampionStatus(championData));

                emptyTile.DeployChampion(champion);
                player.playerField.Champions.Add(champion);
            }
            else
            {
                Debug.LogWarning("Failed to spawn a champion.");
            }
        }
        else
        {
            Debug.LogWarning($"Player {playerRef} not found in GameManager.");
        }
    }
}
