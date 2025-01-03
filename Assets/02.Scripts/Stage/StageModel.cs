using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class StageModel : NetworkBehaviour
{
    [Inject] private readonly ProgressTimer progressTimer;
    [Inject] private readonly PlayerManager playerManager;
    [Inject] private readonly StageStateManager stageStateManager;

    public int StageIndex { get; set; } = 0;
    public int RoundIndex { get; set; } = 0;
    public string StageName => $"{StageIndex}-{RoundIndex}";

    public float StageProgress { get; private set; }

    private StageStateBehaviour ActiveStageState;


    public Dictionary<PlayerRef, PlayerRef> matchingPairs = new Dictionary<PlayerRef, PlayerRef>();
    [Networked] public PlayerRef matchingPlayer { get; set; }

    public void OnStageEnter(StageStateBehaviour stageState)
    {
        ActiveStageState = stageState;

        switch (stageState)
        {
            case BattleReadyState:
                if (IsLastRound())
                {
                    StageIndex++;
                    RoundIndex = 1;
                }
                else
                {
                    RoundIndex++;
                }
                break;
        }
    }

    public bool IsLastRound()
    {
        return RoundIndex > 1;
    }

    public void DoMatching(List<PlayerRef> allPlayers)
    {
        // 여기서 remainPlayers를 만들고, 랜덤 매칭 로직 수행
        List<PlayerRef> remainPlayers = new List<PlayerRef>(allPlayers);
        matchingPairs.Clear();

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

            matchingPairs.Add(player1, player2);
        }

        while (remainPlayers.Count > 1)
        {
            int index1 = Random.Range(0, remainPlayers.Count);
            PlayerRef player1 = remainPlayers[index1];
            remainPlayers.RemoveAt(index1);

            int index2 = Random.Range(0, remainPlayers.Count);
            PlayerRef player2 = remainPlayers[index2];
            remainPlayers.RemoveAt(index2);

            matchingPairs.Add(player1, player2);
            //model.matchingPairs.Add(player2, player1);
        }

        // matchingPairs에 결과 저장
    }

    public void MovePlayerToField(Player player)
    {
        if (player == null) return;

        // 예시로 "PlayerField"로 이동하는 로직
        // (실제 구현은 Player 내부 로직에 따라 달라집니다)
        player.MoveToPlayerField(player.playerField);
    }
}
