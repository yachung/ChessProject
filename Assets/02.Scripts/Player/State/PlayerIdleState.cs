using UnityEngine;
using Fusion.Addons.FSM;

public class PlayerIdleState : PlayerStateBehaviour
{
    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (Controller.CheckDistance() > 2f)
        {
            Machine.TryActivateState<PlayerWalkState>();
        }
    }
}
