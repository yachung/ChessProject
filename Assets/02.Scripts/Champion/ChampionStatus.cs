using Fusion;
using System;
using UnityEngine;

public struct ChampionStatus : INetworkStruct
{
    public ChampionStatus(PlayerRef inputAuthority, ChampionData data)
    {
        InputAuthority = inputAuthority;
        Name = data.championName;
        Grade = 1;
        Cost = data.cost;
        HealthPoint = data.health;
        AttackPower = data.attackDamage;
        Range = data.range;
        Speed = data.speed;
        IsDeath = false;
        IsDrag = false;

        BattleCoord = Vector2Int.zero;
        ReadyCoord = Vector2Int.zero;
    }

    //public event Action<ChampionStatus> OnStatusChanged;
    public PlayerRef InputAuthority;
    public NetworkString<_32> Name;
    public Vector2Int BattleCoord;
    public Vector2Int ReadyCoord;

    public int Grade;
    public int Cost;
    public float HealthPoint;
    public float AttackPower;
    public int Range;
    public float Speed;
    public bool IsDeath;
    public bool IsDrag;
    //[Networked]  { get; set; }

}
