using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public enum ChampionType
{
    Warrior,
    Magician,
    Archer,
}

public struct ChampionStatus : INetworkStruct
{
    public ChampionStatus(ChampionData data)
    {
        CharacteristicClass = data.championType;
        Name = data.championName;
        Grade = 1;
        Cost = data.cost;
        HealthPoint = data.health;
        AttackPower = data.attackDamage;
        Range = data.range;
        Speed = data.speed;
    }

    public ChampionType CharacteristicClass;
    public NetworkString<_32> Name;
    public int Grade;
    public int Cost;
    public float HealthPoint;
    public float AttackPower;
    public int Range;
    public float Speed;
}


public abstract class Champion : NetworkBehaviour
{
    public ChampionStatus status;
    //public abstract ChampionType CharacteristicClass {  get; protected set; }
    //public abstract string Name { get ; protected set; }
    //public abstract int Grade { get; protected set; }
    //public abstract int Cost { get; protected set; }
    //public abstract float HealthPoint { get; protected set; }
    //public abstract float AttackPower { get; protected set; }
    //public abstract int Range { get; protected set; }
    //public abstract float Speed { get; protected set; }

    [Networked, OnChangedRender(nameof(OnIsDeathChanged))] public bool IsDeath { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChanged))] public float RemainHp { get; set; }

    [SerializeField] public GameObject Model;

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }

    public bool Busy { get; set; }

    public Vector2Int ReadyCoord;
    public Vector2Int BattleCoord;

    private void Awake()
    {
        Controller = GetComponentInChildren<ChampionController>();
        Animator = GetComponentInChildren<Animator>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DataInitialize(ChampionStatus status)
    {
        //if (Object.HasInputAuthority)
        //{
        //}
        
        this.status = status;

        if (Runner.IsServer)
        {
            RemainHp = status.HealthPoint;
            IsDeath = false;
        }
    }

    public void Damage(float attackPower)
    {
        RemainHp -= attackPower;
    }

    private void OnHpChanged()
    {
        Controller.OnHpChanged(RemainHp / status.HealthPoint);

        if (Runner.IsServer && RemainHp <= 0)
        {
            Death();
        }
    }

    private void OnIsDeathChanged()
    {
        if (IsDeath)
        {
            Model.SetActive(false);
        }
        else
        {
            Model.SetActive(true);
        }
    }

    public void Respawn()
    {
        IsDeath = false;
        RemainHp = status.HealthPoint;
    }

    public void Death()
    {
        IsDeath = true;
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
