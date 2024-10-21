using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ChampionManager : NetworkBehaviour
{
    private GameManager _gameManager;
    private List<Champion> _champions = new List<Champion>();

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public override void Spawned()
    {
        //Object.AssignInputAuthority(Runner.LocalPlayer);
    }

    // 클라이언트가 소환 요청을 보내면 서버에서 소환을 처리
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SummonChampion(NetworkString<_32> name, PlayerRef player)
    {
        if (Runner.IsServer)
        {
            SummonChampion(name.Value, player);
        }
    }

    private void SummonChampion(string name, PlayerRef player)
    {
        if (_gameManager.allPlayers.TryGetValue(player, out Player playerData))
        {
            ChampionData championData = Resources.Load($"Data/{name}Data") as ChampionData;

            playerData.Gold -= championData.cost;

            NetworkPrefabRef championPrefab = championData.championPrefab;
            
            Vector3 spawnPosition;

            Tile emptyTile = playerData.playerField.GetEmptyWaitField();

            if (emptyTile == null)
            {
                Debug.LogWarning("Failed to spawn a champion.");
                return;
            }
            else
                spawnPosition = emptyTile.DeployPoint;

            if (Runner.Spawn(championPrefab, spawnPosition, Quaternion.identity, player).TryGetComponent<Champion>(out var champion))
            {
                champion.RPC_DataInitialize(new ChampionStatus(championData));

                emptyTile.DeployChampion(champion);
                playerData.playerField.Champions.Add(champion);
                _champions.Add(champion);
            }
            else
            {
                Debug.LogWarning("Failed to spawn a champion.");
            }
        }
        else
        {
            Debug.LogWarning($"Player {player} not found in GameManager.");
        }
    }
}
