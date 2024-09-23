using Fusion.Addons.FSM;

public class SearchState : ChampionStateBehaviour
{
    protected override void OnEnterState()
    {
        base.OnEnterState();

        championController.EnemySearch(out Champion target);
        if (target != null)
        {
            if (championController.InRangeCheck())
            {
                Machine.TryActivateState<AttackState>();
            }
            else
            {
                Machine.TryActivateState<MoveState>();
            }
        }
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        championController.Animator.SetBool("Idle", true);
    }
}
