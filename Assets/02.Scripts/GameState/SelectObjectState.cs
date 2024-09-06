using Fusion.Addons.FSM;
using System.Linq;

public class SelectObjectState : StateBehaviour
{
    protected override bool CanEnterState()
    {
        return base.CanEnterState();
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        //if (HasStateAuthority)
        //{
        //    GameManager.Instance.selectField.PlayerSetPosition(GameManager.Instance.allPlayers.Values.ToArray());
        //}
    }

    protected override bool CanExitState(StateBehaviour nextState)
    {
        return base.CanExitState(nextState);
    }

    protected override void OnExitState()
    {
        base.OnExitState();
    }
}
