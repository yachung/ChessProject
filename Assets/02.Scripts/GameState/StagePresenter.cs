using VContainer;
using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class StagePresenter : NetworkBehaviour
{
    private StageView view;
    private StageModel stageModel;
    private PlayerManager playerManager;
    private ProgressTimer timer;

    [Inject]
    public void Constructor(StageView stageView, StageModel stageModel, PlayerManager playerManager)
    {
        this.view = stageView;
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


    private void Update()
    {
        float ratio = timer.GetRemainingTimeRatio();
        if (ratio < 0f)
            return;

        // 0~1 비율을 뷰에 표시
        view.UpdateProgressBar(ratio);
    }

    //public override void Spawned()
    //{
    //    //Object.AssignInputAuthority(Runner.LocalPlayer);
    //    Runner.SetIsSimulated(Object, true);
    //}

    //public override void FixedUpdateNetwork()
    //{
    //    float progress = model.Get;

    //    if (model.TransitionTimer.IsRunning)
    //    {
    //        UpdateProgressBar(ActiveStageState);
    //    }
    //}

    public void OnClickPlayerList(Player player)
    {
        stageModel.MovePlayerToField(player);
    }

    public void UpdatePlayerList()
    {
        var players = playerManager?.PlayerList;
        view.UpdatePlayerList(players);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_BattleResult(PlayerRef winnerRef, PlayerRef loserRef)
    {
        if (!Runner.IsServer)
            return;

        int Damage = 0;
        Player winner = playerManager.GetPlayer(winnerRef);
        Player loser = playerManager.GetPlayer(loserRef);


        Damage = winner.Level + 1;
        loser.Hp -= Damage;
    }

    public PlayerRef GetMatchingPlayer(PlayerRef playerRef)
    {
        return stageModel.matchingPairs[playerRef];
    }

    public void MatchingPlayer()
    {
        var allPlayers = playerManager.PlayerRefList;
        stageModel.DoMatching(allPlayers);
        // View 갱신 필요하다면 → view.UpdateMatchingResult() 등
    }
}
