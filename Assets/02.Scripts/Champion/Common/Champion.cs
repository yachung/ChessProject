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
        MaxHp = data.health;
        AttackPower = data.attackDamage;
        Range = data.range;
        Speed = data.speed;
    }

    public ChampionType CharacteristicClass;
    public NetworkString<_32> Name;
    public int Grade;
    public int Cost;
    public float MaxHp;
    public float AttackPower;
    public int Range;
    public float Speed;
}


public abstract class Champion : NetworkBehaviour
{
    public ChampionStatus status;

    [Networked, OnChangedRender(nameof(OnIsDeathChanged))] public bool IsDeath { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChanged))] public float RemainHp { get; set; }
    [Networked, OnChangedRender(nameof(OnBattleTeamChanged))] public bool IsAwayTeam { get; set; }

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
        this.status = status;

        if (Runner.IsServer)
        {
            RemainHp = status.MaxHp;
            IsDeath = false;
            IsAwayTeam = false;
        }
    }

    public void OnBattleTeamChanged()
    {
        //if (HasInputAuthority)
        //    return;

        if (IsAwayTeam)
        {
            Controller.HpColorChange(Color.red);
        }
        else
        {
            Controller.HpColorChange(Color.green);
        }
    }

    //public void BattleEnd()
    //{
    //    Controller.HpColorChange(Color.green);
    //}

    public void Damage(float attackPower)
    {
        RemainHp -= attackPower;
    }

    private void OnHpChanged()
    {
        Controller.OnHpChanged(RemainHp / status.MaxHp);

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
        RemainHp = status.MaxHp;
    }

    public void Death()
    {
        IsDeath = true;
    }
}
