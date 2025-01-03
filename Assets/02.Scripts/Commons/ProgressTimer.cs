using Fusion;
using UnityEngine;
using VContainer;

public class ProgressTimer : NetworkBehaviour
{
    [Inject] private readonly StageDurationConfig stageDurationConfig;

    [Networked] public TickTimer TransitionTimer { get; private set; }

    // 타이머 설정
    public void SetTransitionTimer(StageStateBehaviour stageState)
    {
        if (!Runner.IsServer)
            return;

        TransitionTimer = TickTimer.CreateFromSeconds(Runner, GetStageDuration(stageState));
    }

    // 타이머가 끝났는지 여부
    public bool IsTransitionTimerCheck()
    {
        return TransitionTimer.ExpiredOrNotRunning(Runner);
    }

    // 스테이지별 시간을 가져오는 로직
    public float GetStageDuration(StageStateBehaviour state)
    {
        float result = 0f;

        switch (state)
        {
            case SelectObjectState:
                result = stageDurationConfig.selectObjectDuration;
                break;
            case BattleReadyState:
                result = stageDurationConfig.battleReadyDuration;
                break;
            case BattleState:
                result = stageDurationConfig.battleDuration;
                break;
            case WinState:
                result = stageDurationConfig.winDuration;
                break;
        }

        return result;
    }

    // 진행도 (0~1)를 계산해서 반환. 
    // 타이머가 끝난 경우 -1f를 반환(혹은 0f 반환도 가능)
    public float GetRemainingTimeRatio(StageStateBehaviour stageState)
    {
        if (!TransitionTimer.IsRunning)
            return -1f;

        float totalTime = GetStageDuration(stageState);
        float remainTime = TransitionTimer.RemainingTime(Runner).GetValueOrDefault();

        // 전체 중에서 남은 비율
        float ratio = remainTime / totalTime;
        return ratio;
    }
}
