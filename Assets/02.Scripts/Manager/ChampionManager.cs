using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ChampionManager : NetworkBehaviour
{
    private TileManager tileManager;
    private PlayerField playerField;
    private GameManager gameManager;
    private List<Champion> champions = new List<Champion>();

    [Inject]
    public void Construct(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Initialize(TileManager tileManager, PlayerField playerField)
    {
        this.tileManager = tileManager;
        this.playerField = playerField;
    }

    //#region 챔피언 소환

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RPC_SummonChampion(NetworkString<_32> name, PlayerRef player)
    //{
    //    if (Runner.IsServer)
    //    {
    //        SummonChampion(name.Value, player);
    //    }
    //}

    //private void SummonChampion(string name, PlayerRef playerRef)
    //{
    //    if (gameManager.allPlayers.TryGetValue(playerRef, out Player player))
    //    {
    //        ChampionData championData = Resources.Load<ChampionData>($"Data/{name}Data");

    //        if (championData == null)
    //        {
    //            Debug.LogWarning($"Champion data not found for {name}.");
    //            return;
    //        }

    //        if (player.Gold < championData.cost)
    //        {
    //            Debug.LogWarning($"Player {playerRef} does not have enough gold.");
    //            return;
    //        }

    //        player.Gold -= championData.cost;

    //        Tile emptyTile = player.playerField.GetEmptyWaitField();
    //        if (emptyTile == null)
    //        {
    //            Debug.LogWarning("Failed to spawn a champion. No empty tiles available.");
    //            return;
    //        }

    //        Vector3 spawnPosition = emptyTile.DeployPoint;

    //        if (Runner.Spawn(championData.championPrefab, spawnPosition, Quaternion.identity, playerRef)
    //            .TryGetComponent<Champion>(out var champion))
    //        {
    //            champion.RPC_DataInitialize(new ChampionStatus(championData));
    //            emptyTile.DeployChampion(champion);
    //            player.playerField.Champions.Add(champion);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Failed to spawn a champion prefab.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"Player {playerRef} not found in GameManager.");
    //    }
    //}
    //
    //#endregion

    #region 챔피언 관리

    public void RespawnChampions()
    {
        foreach (var champion in champions)
        {
            champion.Respawn();
            Tile targetTile = tileManager.GetTile(champion.ReadyCoord);
            targetTile?.DeployChampion(champion);
        }
    }

    //public void ClearChampions()
    //{
    //    foreach (var champion in champions)
    //    {
    //        champion.RemoveFromField();
    //    }

    //    champions.Clear();
    //}

    #endregion
}
