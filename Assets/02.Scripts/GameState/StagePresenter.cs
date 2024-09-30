
using VContainer;
using UnityEngine;
using Fusion;

public class StagePresenter : NetworkBehaviour
{
    private StageView view;
    private StageModel model;

    private StageStateBehaviour ActiveStageState;

    [Inject]
    public void Constructor(StageView stageView, StageModel stageModel)
    {
        this.view = stageView;
        this.model = stageModel;

        this.view.SetPresenter(this);
    }

    public void StageViewInitialize(StageStateBehaviour stageState)
    {
        if (model.GetStageDuration(stageState) > 0)
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

    public void OnStageEnter(StageStateBehaviour stageState)
    {
        ActiveStageState = stageState;

        switch (stageState)
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

    public void UpdateProgressBar(StageStateBehaviour stageState)
    {
        if (!model.TransitionTimer.IsRunning)
            return;

        float RemainTime = model.TransitionTimer.RemainingTime(Runner).GetValueOrDefault() / model.GetStageDuration(stageState);

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

    public void Server_SetTransitionTimer(StageStateBehaviour stageState)
    {
        if (!Runner.IsServer)
            return;

        model.TransitionTimer = TickTimer.CreateFromSeconds(Runner, model.GetStageDuration(stageState));
    }

    public void OnClickPlayerList(Player player)
    {
        player.MoveToPlayerField(player.playerField);
    }
}
