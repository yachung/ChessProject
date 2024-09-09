using Cysharp.Threading.Tasks;
using Fusion;
using System;

public class SceneLoader
{
    public enum SceneType
    {
        Lobby = 0,
        InGame = 1,
    }

    public async void Server_OnGameStarted(NetworkRunner runner, Action callBack = null)
    {
        if (!runner.IsSceneAuthority)
            return;

        await Server_LoadSceneAsync(runner, SceneType.InGame);

        callBack?.Invoke();
        //gameState.Server_SetState<SelectObjectState>();
    }

    public async UniTask Server_LoadSceneAsync(NetworkRunner runner, SceneType sceneType)
    {
        if (runner.IsSceneAuthority)
        {
            SceneRef sceneRef = SceneRef.FromIndex((int)sceneType);
            await runner.LoadScene(sceneRef);
        }
    }
}
