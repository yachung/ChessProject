using Fusion;
using VContainer;

public class LobbyInitializer : NetworkBehaviour
{
    private LobbyPresenter _lobbyPresenter;

    // DI 컨테이너를 통해 LobbyPresenter 주입
    [Inject]
    public void Construct(LobbyPresenter lobbyPresenter)
    {
        _lobbyPresenter = lobbyPresenter;
    }

    private void Start()
    {
    }

    public override void Spawned()
    {
        _lobbyPresenter.Initialize(Runner.IsServer);
        Runner.AddCallbacks(_lobbyPresenter);
    }
}
