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
    public abstract float AttackPoint {  get; protected set; }
    public abstract int Range { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract bool IsDeath { get; protected set; }

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<ChampionController>();
        Animator = GetComponentInChildren<Animator>();
    }
}
