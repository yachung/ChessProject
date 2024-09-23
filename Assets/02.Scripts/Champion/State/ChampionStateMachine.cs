using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;

public class ChampionStateMachine : NetworkBehaviour, IStateMachineOwner
{
    public ChampionController ChampionController { get; private set; }

    private StateMachine<ChampionStateBehaviour> stateMachine;

    private void Awake()
    {
        ChampionController = GetComponent<ChampionController>();
    }

    public void CollectStateMachines(List<IStateMachine> stateMachines)
    {
        var states = GetComponentsInChildren<ChampionStateBehaviour>(true);

        foreach (var state in states)
        {
            state.championController = this.ChampionController;
        }

        stateMachine = new StateMachine<ChampionStateBehaviour>("Champion State", states);

        stateMachines.Add(stateMachine);
    }
}
