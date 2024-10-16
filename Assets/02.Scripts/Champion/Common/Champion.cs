using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public enum ChampionType
{
    Warrior,
    Magician,
    Archer,
}

public abstract class Champion : NetworkBehaviour
{
    public abstract ChampionType CharacteristicClass {  get; protected set; }
    public abstract string Name { get ; protected set; }
    public abstract int Grade { get; protected set; }
    public abstract int Cost { get; protected set; }
    public abstract float HealthPoint { get; protected set; }
    public abstract float AttackPower {  get; protected set; }
    public abstract int Range { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract bool IsDeath { get; protected set; }

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public bool Busy { get; set; }

    public Vector2Int ReadyCoord;
    public Vector2Int BattleCoord;

    private void Awake()
    {
        Controller = GetComponent<ChampionController>();
        Animator = GetComponentInChildren<Animator>();
    }

    public void Damage(float attackPower)
    {
        HealthPoint -= attackPower;
    }

    public void MoveTowardTile(Vector2Int coord)
    {

    }

    public void DataInitialize(ChampionData data)
    {
        CharacteristicClass = data.championType;
        Name = data.championName;
        Grade = 1;
        HealthPoint = data.health;
        AttackPower = data.attackDamage;
        Speed = data.speed;
        Range = data.range;
        Cost = data.cost;
        IsDeath = false;
    }

    //public override void Spawned()
    //{
    //    base.Spawned();

    //    if (HasInputAuthority)
    //    {
    //        Runner
    //    }
    //}
}
