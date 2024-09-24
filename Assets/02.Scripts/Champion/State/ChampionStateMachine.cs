using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;

public class ChampionStateMachine : NetworkBehaviour, IStateMachineOwner
{
    public Champion Champion { get; private set; }

    private StateMachine<ChampionStateBehaviour> stateMachine;

    private void Awake()
    {
        Champion = GetComponent<Champion>();
    }

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        var states = GetComponentsInChildren<ChampionStateBehaviour>(true);

        foreach (var state in states)
        {
            state.champion = this.Champion;
        }

        stateMachine = new StateMachine<ChampionStateBehaviour>("Champion State", states);

        stateMachines.Add(stateMachine);
    }
}
