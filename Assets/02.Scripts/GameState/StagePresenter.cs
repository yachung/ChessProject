
using VContainer;
using UnityEngine;
using Fusion;

public class StagePresenter : NetworkBehaviour
{
    [Inject] private readonly StageView view;
    [Inject] private readonly StageModel model;

    private StageStateBehaviour ActiveStageState;

    public void StageViewInitialize(StageStateBehaviour state)
    {
        if (model.GetStageDuration(state) > 0)
            view.ShowUI();
        else
            view.HideUI();
    }

    public override void Spawned()
    {
        Runner.SetIsSimulated(Object, true);
    }

    public override void FixedUpdateNetwork()
    {
        if (model.TransitionTimer.IsRunning)
        {
            UpdateProgressBar(ActiveStageState);
        }
    }

    public void OnStageEnter(StageStateBehaviour state)
    {
        ActiveStageState = state;

        switch (state)
        {
            case BattleReadyState:
                if (IsLastRound())
                {
                    model.StageIndex++;
                    model.RoundIndex = 1;
                }
                else
                {
                    model.RoundIndex++;
                }
                break;
        }

        view.DisplayStageName(model.StageName);
    }

    public void UpdateProgressBar(StageStateBehaviour state)
    {
        if (!model.TransitionTimer.IsRunning)
            return;

        float RemainTime = model.TransitionTimer.RemainingTime(Runner).GetValueOrDefault() / model.GetStageDuration(state);

        view.UpdateProgressBar(RemainTime);
    }

    public bool IsTransitionTimerCheck()
    {
        return model.TransitionTimer.ExpiredOrNotRunning(Runner);
    }

    public bool IsLastRound()
    {
        return model.RoundIndex > 1;
    }

    public void Server_SetTransitionTimer(StageStateBehaviour state)
    {
        if (!Runner.IsServer)
            return;

        model.TransitionTimer = TickTimer.CreateFromSeconds(Runner, model.GetStageDuration(state));
    }
}
