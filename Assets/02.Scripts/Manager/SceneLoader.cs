using Cysharp.Threading.Tasks;
using Fusion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public enum SceneType
{
    Login = 0,
    Lobby = 1,
    InGame = 2,
}

public class SceneLoader : INetworkSceneManager
{
    private bool _isLoading;

    public bool IsBusy => false;

    public Scene MainRunnerScene => throw new NotImplementedException();

    public NetworkRunner Runner { get; private set; }

    //public async void Server_OnGameStarted(NetworkRunner runner, Action sceneLoadComplete = null)
    //{
    //    if (!runner.IsSceneAuthority)
    //        return;

    //    await Server_LoadSceneAsync(runner, SceneType.InGame);

    //    sceneLoadComplete?.Invoke();
    //}

    //public async UniTask Server_LoadSceneAsync(NetworkRunner runner, SceneType sceneType)
    //{
    //    if (!runner.IsSceneAuthority)
    //        return;

    //    using (LifetimeScope.EnqueueParent(parent))
    //    {
    //        SceneRef sceneRef = SceneRef.FromIndex((int)sceneType);
    //        await runner.LoadScene(sceneRef);
    //    }
    //}

    public void Initialize(NetworkRunner runner)
    {
        Runner = runner;
    }

    public void Shutdown()
    {
        Runner = null;
    }

    public async void LoadScene(SceneType sceneType, Action loadingComplete = null)
    {
        await LoadScene(GetSceneRef(sceneType), new NetworkLoadSceneParameters());

        loadingComplete?.Invoke();
    }

    public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, NetworkLoadSceneParameters parameters)
    {
        return Runner.LoadScene(sceneRef);
    }

    public NetworkSceneAsyncOp UnloadScene(SceneRef sceneRef)
    {
        return Runner.UnloadScene(sceneRef);
    }

    public SceneRef GetSceneRef(SceneType sceneType)
    {
        return SceneRef.FromIndex((int)sceneType);
    }

    public SceneRef GetSceneRef(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    public SceneRef GetSceneRef(string sceneNameOrPath)
    {
        throw new NotImplementedException();
    }

    public bool OnSceneInfoChanged(NetworkSceneInfo sceneInfo, NetworkSceneInfoChangeSource changeSource)
    {
        return false;
    }



    /// <summary>
    /// Fusion2 멀티피어 지원용
    /// </summary>

    public bool IsRunnerScene(Scene scene)
    {
        throw new NotImplementedException();
    }

    public bool TryGetPhysicsScene2D(out PhysicsScene2D scene2D)
    {
        throw new NotImplementedException();
    }

    public bool TryGetPhysicsScene3D(out PhysicsScene scene3D)
    {
        throw new NotImplementedException();
    }

    public void MakeDontDestroyOnLoad(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public bool MoveGameObjectToScene(GameObject gameObject, SceneRef sceneRef)
    {
        throw new NotImplementedException();
    }


}
