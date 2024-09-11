using Fusion;
using UnityEngine;

enum MyButtons
{
    isMove = 0,
    isDrag = 1,
    isRefresh = 2,
    isAddExp = 3,
}

/// <summary>
/// 클라이언트는 사용자에게 즉각적인 피드백을 제공하기 위해 입력을 로컬로 적용할 수도 있지만, 해당 입력은 호스트에 의해 무시될수 있는 로컬 예측.
/// 사용자로부터 입력을 수집하기 전에 입력을 유지하기 위한 데이터 구조체
/// </summary>
public struct NetworkInputData : INetworkInput
{
    public Vector2 movePosition;
    public NetworkButtons buttons;
}
