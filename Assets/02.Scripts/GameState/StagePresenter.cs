using VContainer;
using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class StagePresenter
{
    private IStageView view;
    private ProgressTimer progressTimer;

    private StageModel stageModel;
    private PlayerManager playerManager;

    [Inject]
    public void Constructor(IStageView view, StageModel stageModel, PlayerManager playerManager)
    {
        this.view = view;
        this.stageModel = stageModel;
        this.playerManager = playerManager;

        this.view.SetPresenter(this);
    }

    public void InitializeView()
    {
        view.DisplayStageName(stageModel.StageName);
        view.UpdateProgressBar(stageModel.StageProgress);
        view.ShowUI();
    }

    public void UpdateTimer(StageStateBehaviour currentState)
    {
        float ratio = progressTimer.GetRemainingTimeRatio(currentState);

        if (ratio < 0f)
            return;

        view.UpdateProgressBar(ratio);
    }

    public void OnClickPlayerList(Player player)
    {
        stageModel.MovePlayerToField(player);
    }

    public void UpdatePlayerList()
    {
        var players = playerManager?.PlayerList;
        view.UpdatePlayerList(players);
    }
}
