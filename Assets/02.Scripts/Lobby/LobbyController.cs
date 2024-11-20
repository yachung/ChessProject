using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Michsky.MUIP;
using Cysharp.Threading.Tasks;
using VContainer;

public class LobbyController : MonoBehaviour
{
    [Inject] private readonly RoomModel roomModel;
    [Inject] private readonly SceneLoader sceneLoader;
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private ButtonManager btn_FindOrCreateRoom;

    private NetworkRunner runner;
    
    private void Awake()
    {
        btn_FindOrCreateRoom.onClick.AddListener(OnStartGameClicked);
    }

    private async void OnStartGameClicked()
    {
        btn_FindOrCreateRoom.isInteractable = false;

        if (!runner)
        {
            // 기존에 생성된 NetworkRunner가 있는지 찾기
            runner = FindAnyObjectByType<NetworkRunner>();

            if (!runner)
                // 없으면 새로운 NetworkRunner 인스턴스 생성
                runner = Instantiate(networkRunnerPrefab);
        }

        runner.ProvideInput = true;

        var result = await StartRunnerAsync(runner, GameMode.AutoHostOrClient);

        if (result.Ok)
        {
            // 방에 입장 성공하면 Room으로 이동
            roomModel.IsFindRoom = true;
        }
        else
        {
            roomModel.IsFindRoom = false;
            Debug.LogError($"Failed to FindOrCreateRoom: {result.ShutdownReason}");
            btn_FindOrCreateRoom.isInteractable = true;
        }
    }

    private async UniTask<StartGameResult> StartRunnerAsync(NetworkRunner runner, GameMode gameMode)
    {
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();

        if (scene.IsValid)
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);

        return await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            PlayerCount = 8,
            Scene = sceneInfo,
            SceneManager = runner.GetComponent<NetworkSceneManagerDefault>(),
            //Address = NetAddress.Any() // IP 주소를 자동으로 할당
        });
    }
}
