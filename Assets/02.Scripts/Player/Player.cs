using Fusion;
using UnityEngine;
using VContainer;

public class Player : NetworkBehaviour
{
    private PlayerInfo playerInfo;
    private PlayerController playerController;
    private Camera mainCamera;
    public PlayerField playerField { get; private set; }

    public int Level { get; private set; }
    public int Exp {  get; private set; }
    public int Gold { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInChildren<PlayerController>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            // Todo: Spawn은 네트워크 Initialzie와 같은의미이며,
            // 여기서 값을 받아온다는건 서버에 요청을 보내서 받아온다는 의미.
            // 현재는 서버에서 플레이어를 생성함과 동시에 값을 보내주고있다.
            // networkmanager.instance.rpc_getplayerinformation


            // Runner.AddCallbacks(inputManager);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayerFieldInitialize(PlayerField playerField)
    {
        this.playerField = playerField;
    }

    public void MoveToPlayerField(PlayerField playerField)
    {
        PlayerTeleport(playerField.transform.position, playerField.cameraPosition);
    }

    public void PlayerTeleport(Vector3 position, Vector3 CameraPosition)
    {
        playerController.PlayerTeleport(position);

        SetPlayerCamera(CameraPosition);
    }

    public void SetPlayerCamera(Vector3 targetPosition)
    {
        mainCamera.transform.position = targetPosition;
    }
}
