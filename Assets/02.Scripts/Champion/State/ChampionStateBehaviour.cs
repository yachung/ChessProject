using Fusion.Addons.FSM;
using UnityEngine;

public abstract class ChampionStateBehaviour : StateBehaviour<ChampionStateBehaviour>
{
    [HideInInspector]
    public Champion champion;

    protected virtual string StateName { get; set; }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        champion.Animator.SetBool(StateName, true);
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        champion.Animator.SetBool(StateName, false);
    }
}
