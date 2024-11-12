using Cysharp.Threading.Tasks;
using Fusion;
using System;
using VContainer;
using VContainer.Unity;

public class SceneLoader
{
    public enum SceneType
    {
        Login = 0,
        Lobby = 1,
        InGame = 2,
    }

    [Inject] private readonly LifetimeScope parent;

    public async void Server_OnGameStarted(NetworkRunner runner, Action sceneLoadComplete = null)
    {
        if (!runner.IsSceneAuthority)
            return;

        await Server_LoadSceneAsync(runner, SceneType.InGame);

        sceneLoadComplete?.Invoke();
    }

    public async UniTask Server_LoadSceneAsync(NetworkRunner runner, SceneType sceneType)
    {
        if (!runner.IsSceneAuthority)
            return;

        using (LifetimeScope.EnqueueParent(parent))
        {
            SceneRef sceneRef = SceneRef.FromIndex((int)sceneType);
            await runner.LoadScene(sceneRef);
        }
    }
}
