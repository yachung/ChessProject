using UnityEngine;
using Fusion.Addons.FSM;
using VContainer;

public class PregameState : StateBehaviour
{
    private LobbyPresenter _lobbyPresenter;

    [Inject]
    public void Constructor(LobbyPresenter lobbyPresenter)
    {
        _lobbyPresenter = lobbyPresenter;
    }

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
        //_lobbyPresenter.Initialize();
        //uiRoom.Initialize(Runner);
        //GameManager.Instance.OnPlayerInfosChanged();
    }

    protected override bool CanExitState(StateBehaviour nextState)
    {
        return base.CanExitState(nextState);
    }

    protected override void OnExitState()
    {
        base.OnExitState();
    }
}
