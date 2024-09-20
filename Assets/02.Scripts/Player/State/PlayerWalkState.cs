using Fusion.Addons.FSM;

public class PlayerWalkState : PlayerStateBehaviour
{
    private float distance;

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        Controller.Animator.SetBool("Move", true);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        distance = Controller.MoveToward();

        if (distance <= 0.5f)
        {
            Machine.TryActivateState<PlayerIdleState>();
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        Controller.Animator.SetBool("Move", false);
    }
}
