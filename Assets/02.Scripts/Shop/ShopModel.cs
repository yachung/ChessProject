using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel : NetworkBehaviour
{
    [SerializeField] private List<ChampionData> championDatas;
    public List<ChampionData> ChampionDatas => championDatas;
    public Player player => GameManager.Instance.LocalPlayer;

    private readonly int MaxCardCount = 5;

    public List<ChampionData> GetRandomChampions()
    {
        if (championDatas.Count == 0)
        {
            return null; // 리스트가 비어있는 경우 null 반환
        }

        List<ChampionData> result = new List<ChampionData>();

        for (int i = 0; i < MaxCardCount; ++i)
        {
            // 리스트에서 랜덤 인덱스 선택
            int randomIndex = Random.Range(0, championDatas.Count);
            result.Add(championDatas[randomIndex]);
        }

        return result; // 선택된 아이템 반환
    }
}
