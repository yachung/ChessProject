using Fusion;
using UnityEngine;

public struct ChampionStatus
{
    public ChampionStatus(NetworkString<_32> name, int grade, int cost, float healthPoint, float attackPower, int range, float speed, bool isDeath)
    {
        Name = name;
        Grade = grade;
        Cost = cost;
        HealthPoint = healthPoint;
        AttackPower = attackPower;
        Range = range;
        Speed = speed;
        IsDeath = isDeath;
    }

    public NetworkString<_32> Name { get; set; }
    public int Grade { get; set; }
    public int Cost { get; set; }
    public float HealthPoint { get; set; }
    public float AttackPower { get; set; }
    public int Range { get; set; }
    public float Speed { get; set; }
    public bool IsDeath { get; set; }


}
