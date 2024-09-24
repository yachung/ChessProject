using Fusion.Addons.FSM;

public class SearchState : ChampionStateBehaviour
{
    protected override void OnEnterState()
    {
        base.OnEnterState();

        champion.Controller.EnemySearch(out Champion target);
        if (target != null)
        {
            if (champion.Controller.InRangeCheck())
            {
                Machine.TryActivateState<AttackState>();
            }
            else
            {
                Machine.TryActivateState<MoveState>();
            }
        }
    }
}
