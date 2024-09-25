using UnityEngine;

public class FootMan : Champion
{
    public override ChampionType CharacteristicClass { get; protected set; }
    public override string Name { get; protected set; }
    public override int Grade { get; protected set; }
    public override int Cost { get; protected set; }
    public override float HealthPoint { get; protected set; }
    public override float AttackPoint { get; protected set; }
    public override int Range { get; protected set; }
    public override float Speed { get; protected set; }
}
