using UnityEngine;
using Fusion;

[CreateAssetMenu(fileName = "NewChampionData", menuName = "Game/Champion Data")]
public class ChampionData : ScriptableObject
{
    // 기본 챔피언 정보
    public string championID; // 고유 ID
    public NetworkPrefabRef championPrefab; // 챔피언 프리팹
    public string championName; // 챔피언 이름
    public ChampionType championType; // 챔피언 타입 (예: 전사, 마법사)
    public int championLevel; // 챔피언 등급 또는 레벨 (1성, 2성, 3성)

    // 챔피언 능력치
    public float health;
    public float attackDamage;
    public float abilityPower;
    public float skillCooldown;

    // 카드 비주얼 정보
    public Sprite cardImage; // 카드 이미지
    public string description; // 카드 설명

    // 게임 내 상호작용 정보
    public int cost; // 소환 비용
    public Vector3 spawnPosition; // 스폰 위치 (상점이나 필드에서 설정)
}