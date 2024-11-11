using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    /// <summary>
    /// Runtime 전에 초기화 해야될 세팅이 있을 경우 사용
    /// </summary>
    //[RuntimeInitializeOnLoadMethod]
    static void InitializeApplication()
    {
        SceneManager.LoadScene("Lobby");
        GameObject gameObject = new GameObject("BootStrap", typeof(Bootstrap));
        gameObject.GetComponent<Bootstrap>().networkRunnerPrefab = Resources.Load<NetworkRunner>("Network/NetworkRunner");
        DontDestroyOnLoad(gameObject);
    }

    private NetworkRunner networkRunnerPrefab;
    private NetworkRunner runner;
    private List<NetworkRunner> clients = new List<NetworkRunner>();

    /// <summary>
    /// Network 이용자 모드 설정
    /// </summary>
    /// <param name="gameMode"></param>
    public async void StartGame(GameMode gameMode)
    {
        runner = Instantiate(networkRunnerPrefab);
        runner.ProvideInput = true;
        await StartRunnerAsync(runner, gameMode);
        Debug.Log($"[{runner.name}] : Started with {gameMode}...");

        switch (gameMode)
        {
            case GameMode.Server:
                break;
            case GameMode.Host:
            case GameMode.Client:
            case GameMode.AutoHostOrClient:
                clients.Add(runner);
                break;
        }
    }

    async Task<StartGameResult> StartRunnerAsync(NetworkRunner runner, GameMode gameMode)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/01.Scenes/Lobby.unity");
        // 이 씬을 참조할 정보를 담은 구조체
        SceneRef sceneRef = SceneRef.FromIndex(buildIndex);
        NetworkSceneInfo networkSceneInfo = new NetworkSceneInfo();
        networkSceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Additive);

        return await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            PlayerCount = 8,
            Scene = networkSceneInfo,
            SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>(),
            //Address = NetAddress.Any() // IP 주소를 자동으로 할당
        });
    }

    private void OnGUI()
    {
        //if (runner != null)
        //    return;

        if (GUI.Button(new Rect(0, 0, 200, 40), "GameStart"))
            StartGame(GameMode.AutoHostOrClient);

        //if (GUI.Button(new Rect(0, 0, 200, 40), "Server"))
        //    StartGame(GameMode.Server);

        //if (GUI.Button(new Rect(0, 40, 200, 40), "Host"))
        //    StartGame(GameMode.Host);

        //if (GUI.Button(new Rect(0, 80, 200, 40), "Client"))
        //    StartGame(GameMode.Client);
    }
}
