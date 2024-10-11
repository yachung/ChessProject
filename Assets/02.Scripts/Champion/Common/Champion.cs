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
    public ChampionStatus ChampionStatus;

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<ChampionController>();
        Animator = GetComponentInChildren<Animator>();
        ChampionStatus = new ChampionStatus();
    }

    public void Damage(float attackPower)
    {
        ChampionStatus.HealthPoint -= attackPower;
    }

    public override void Spawned()
    {
        base.Spawned();

        //if (HasInputAuthority)
        //{
        //    GameManager.Instance.allPlayers
        //}
    }
}
