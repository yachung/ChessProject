using Fusion;
using VContainer;
using VContainer.Unity;

public class LobbyInitializer : NetworkBehaviour
{
    private LobbyPresenter _lobbyPresenter;

    /// <summary>
    /// VContainer에서 MonoBehaviour는 생성자를 지원하지 않으므로 Inject 해줘야 함
    /// </summary>
    /// <param name="lobbyPresenter"></param>
    [Inject]
    public void Constructor(LobbyPresenter lobbyPresenter)
    {
        _lobbyPresenter = lobbyPresenter;
    }

    public override void Spawned()
    {
        //_lobbyPresenter.Initialize(Runner);
        Runner.AddCallbacks(_lobbyPresenter);
    }
}
