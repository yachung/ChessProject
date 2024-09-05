using Fusion;
using VContainer;

public class LobbyInitializer : NetworkBehaviour
{
    private LobbyPresenter _lobbyPresenter;

    // DI �����̳ʸ� ���� LobbyPresenter ����
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
