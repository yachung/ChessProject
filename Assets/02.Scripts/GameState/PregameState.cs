using UnityEngine;
using Fusion.Addons.FSM;
using VContainer;

public class PregameState : StateBehaviour
{
    [Inject] private readonly RoomPresenter _lobbyPresenter;

    protected override bool CanEnterState()
    {
        return base.CanEnterState();
    }

    /// <summary>
    /// 클라이언트는 실행안돼?
    /// </summary>
    protected override void OnEnterState()
    {
        Debug.LogWarning($"EnterState PregameState");
    }

    protected override void OnEnterStateRender()
    {
        _lobbyPresenter.Initialize();
    }

    protected override void OnExitStateRender()
    {
        _lobbyPresenter.DeInitialize();
    }
}
