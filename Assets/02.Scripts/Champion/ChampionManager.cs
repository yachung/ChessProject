using Fusion;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ChampionManager : NetworkBehaviour
{
    [Inject] private readonly GameManager gameManager;

    private List<Champion> champions = new List<Champion>();

    // 클라이언트가 소환 요청을 보내면 서버에서 소환을 처리
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SummonChampion(NetworkPrefabRef championPrefab, Player player)
    {
        if (Object.HasStateAuthority) // 서버에서만 소환 로직 처리
        {
            Champion champion = Runner.Spawn(championPrefab, player.playerField.transform.position, Quaternion.identity).GetComponent<Champion>();
            //champion.Initialize(championData);
            champions.Add(champion);
        }
    }
}