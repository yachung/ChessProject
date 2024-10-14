using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ChampionManager : NetworkBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    private Dictionary<PlayerRef, List<Champion>> playerChampions = new Dictionary<PlayerRef, List<Champion>>();

    public override void Spawned()
    {
        //Object.AssignInputAuthority(Runner.LocalPlayer);
    }

    // 클라이언트가 소환 요청을 보내면 서버에서 소환을 처리
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SummonChampion(NetworkPrefabRef championPrefab, PlayerRef player, NetworkString<_32> name)
    {
        if (Runner.IsServer)
        {
            SummonChampion(championPrefab, player, name.Value);
        }
    }

    private void SummonChampion(NetworkPrefabRef championPrefab, PlayerRef player, string name)
    {
        if (_gameManager.allPlayers.TryGetValue(player, out Player playerData))
        {
            ChampionData championData = Resources.Load($"Data/{name}Data") as ChampionData;

            ChampionStatus championStatus = new ChampionStatus(player, championData);

            Vector3 spawnPosition;

            // 대기열에서 빈 타일을 가져옴
            Tile emptyTile = playerData.playerField.GetEmptyWaitField();

            if (emptyTile == null)
            {
                Debug.LogWarning("Failed to spawn a champion: No empty tile available.");
                return;
            }
            else
            {
                spawnPosition = emptyTile.DeployPoint;
            }

            // 챔피언을 소환하고, 성공 시 상태 정보를 타일에 저장
            if (Runner.Spawn(championPrefab, spawnPosition, Quaternion.identity, player).TryGetComponent<Champion>(out var champion))
            {
                // 타일에 ChampionStatus만 저장
                emptyTile.DeployChampion(championStatus);

                // 챔피언의 물리적 표현 및 동작은 ChampionManager에서 관리
                champion.Initialize(championStatus);

                playerData.playerField.RPC_SetChampion(emptyTile.Coordinate, championStatus);
            }
            else
            {
                Debug.LogWarning("Failed to spawn a champion: Component Champion not found.");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to spawn a champion: Player {player} not found in GameManager.");
        }
    }
}
