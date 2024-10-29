using Fusion;

public struct ChampionStatus : INetworkStruct
{
    public ChampionStatus(ChampionData data)
    {
        CharacteristicClass = data.championType;
        Name = data.championName;
        Grade = 1;
        Cost = data.cost;
        MaxHp = data.health;
        AttackPower = data.attackDamage;
        Range = data.range;
        Speed = data.speed;
        AttackSpeed = data.attackSpeed;
    }

    public ChampionType CharacteristicClass;
    public NetworkString<_32> Name;
    public int Grade;
    public int Cost;
    public float MaxHp;
    public float AttackPower;
    public int Range;
    public float Speed;
    public float AttackSpeed;
}