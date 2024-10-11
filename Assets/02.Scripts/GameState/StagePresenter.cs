using VContainer;
using UnityEngine;
using Fusion;
using System.Collections.Generic;

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
        this.model.OnPlayerChanged = UpdatePlayerList;
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

    public void UpdatePlayerList()
    {
        view.UpdatePlayerList(model.PlayerList);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_BattleResult(PlayerRef winnerRef, PlayerRef loserRef)
    {
        if (!Runner.IsServer)
            return;

        int Damage = 0;

        if (model.PlayerInfos.TryGet(winnerRef, out Player winner))
        {
            //Damage = winner.Level + winner.playerField.ChampionTiles.Count;
            Damage = winner.Level + 1;
        }

        if (model.PlayerInfos.TryGet(loserRef, out Player loser))
        {
            loser.Hp -= Damage;
        }
    }

    public PlayerRef GetMatchingPlayer(PlayerRef playerRef)
    {
        return model.matchingPairs[playerRef];
    }

    public void MatchingPlayer()
    {
        List<PlayerRef> remainPlayers = new List<PlayerRef>(model.PlayerRefList);
        model.matchingPairs.Clear();

        if (remainPlayers.Count % 2 != 0)
        {
            int index1 = Random.Range(0, remainPlayers.Count);
            PlayerRef player1 = remainPlayers[index1];
            remainPlayers.RemoveAt(index1);

            // ToDo: 원래 여기서 remainPlayers.Count는 1이 올 수 없음.
            // 하지만 싱글테스트시 오류나서 임시 수정
            PlayerRef player2;

            if (remainPlayers.Count == 0)
                player2 = player1;
            else
            {
                int index2 = Random.Range(0, remainPlayers.Count);
                player2 = remainPlayers[index2];
            }

            model.matchingPairs.Add(player1, player2);
        }

        while (remainPlayers.Count > 1)
        {
            int index1 = Random.Range(0, remainPlayers.Count);
            PlayerRef player1 = remainPlayers[index1];
            remainPlayers.RemoveAt(index1);

            int index2 = Random.Range(0, remainPlayers.Count);
            PlayerRef player2 = remainPlayers[index2];
            remainPlayers.RemoveAt(index2);

            model.matchingPairs.Add(player1, player2);
            //model.matchingPairs.Add(player2, player1);
        }
    }
}
