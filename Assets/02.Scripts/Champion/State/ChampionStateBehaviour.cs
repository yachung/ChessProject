using Fusion.Addons.FSM;
using UnityEngine;

public abstract class ChampionStateBehaviour : StateBehaviour<ChampionStateBehaviour>
{
    [HideInInspector]
    public Champion champion;

    protected string stateName = string.Empty;

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        champion.Animator.SetBool(stateName, true);
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();

        champion.Animator.SetBool(stateName, false);
    }
}
