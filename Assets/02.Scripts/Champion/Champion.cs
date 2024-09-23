using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public enum CharacteristicClass
{
    Warrior,
    Magician,
    Archer,
}

public class Champion : NetworkBehaviour
{
    public CharacteristicClass CharacteristicClass {  get; private set; }
    public int Grade { get; private set; }
    public int Cost { get; private set; }
    public float HealthPoint { get; private set; }
    public float AttackPoint {  get; private set; }
    public int Range { get; private set; }
    public float Speed { get; private set; }

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    //private void ChampionLogic()
    //{
    //    if (EnemySearch(out var target))
    //    {
    //        if (InRangeCheck(this.Range))
    //        {
    //            if (IsAttackable(target))
    //            {
    //                Attack();
    //            }
    //        }
    //        else
    //        {
    //            MoveNextTile();
    //        }
    //    }
    //}


}
