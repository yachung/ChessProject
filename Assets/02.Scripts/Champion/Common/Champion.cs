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


public abstract class Champion : NetworkBehaviour
{
    public ChampionStatus status;

    [Networked, OnChangedRender(nameof(OnIsDeathChanged))] public bool IsDeath { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChanged))] public float RemainHp { get; set; }
    [Networked, OnChangedRender(nameof(OnBattleTeamChanged))] public bool IsAwayTeam { get; set; }
    [Networked, OnChangedRender(nameof(OnIsMovementBusyChanged))] public bool IsMovementBusy { get; set; }
    [Networked, OnChangedRender(nameof(OnIsAttackChanged))] public bool IsAttack { get; set; }

    [SerializeField] public GameObject Model;

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }


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
        if (IsAwayTeam)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 210f, 0f));
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 30f, 0f));
        }
    }

    public override void Spawned()
    {
        base.Render();

        if (HasInputAuthority)
            return;

        Controller.HpColorChange(Color.red);
    }

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
        Model.SetActive(!IsDeath);
    }

    private void OnIsMovementBusyChanged()
    {
        Animator.SetBool("Move", IsMovementBusy);
    }

    private void OnIsAttackChanged()
    {
        Animator.SetBool("Attack", IsAttack);
    }

    public void Respawn()
    {
        IsDeath = false;
        RemainHp = status.MaxHp;
        IsMovementBusy = false;
        IsAttack = false;

        this.transform.rotation = Quaternion.identity;
    }

    public void Death()
    {
        IsDeath = true;
    }
}
