using UnityEngine;

public class ChampionController : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Champion Target { get; private set; }


    // 탐색
    public bool EnemySearch(out Champion target)
    {
        target = Target;

        return false;
    }

    public bool InRangeCheck()
    {
        return false;
    }

    public bool IsAttackable(Champion target)
    {
        return false;
    }

    private bool IsDead(Champion target)
    {
        return false;
    }

    private void MoveNextTile()
    {

    }

    private void Attack()
    {

    }
}
