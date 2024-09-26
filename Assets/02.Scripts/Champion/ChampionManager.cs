using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ChampionManager : NetworkBehaviour
{
    private GameManager _gameManager;
    private readonly List<Champion> _champions = new List<Champion>();

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
    public void RPC_SummonChampion(NetworkPrefabRef championPrefab, PlayerRef player)
    {
        if (Runner.IsServer)
        {
            SummonChampion(championPrefab, player);
        }
    }

    private void SummonChampion(NetworkPrefabRef championPrefab, PlayerRef player)
    {
        if (_gameManager.allPlayers.TryGetValue(player, out Player playerData))
        {
            Vector3 spawnPosition;

            FieldTile emptyTile = playerData.playerField.GetEmptyWaitField();

            if (emptyTile == null)
            {
                Debug.LogWarning("Failed to spawn a champion.");
                return;
            }
            else
                spawnPosition = emptyTile.deployPoint;

            if (Runner.Spawn(championPrefab, spawnPosition, Quaternion.identity).TryGetComponent<Champion>(out var champion))
            {
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
