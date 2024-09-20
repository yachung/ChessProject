using Fusion;
using UnityEngine;
using VContainer;

public class Player : NetworkBehaviour
{
    private PlayerInfo playerInfo;
    private PlayerController playerController;
    public PlayerField playerField { get; private set; }
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInChildren<PlayerController>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            //Runner.AddCallbacks(inputManager);
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

        RPC_SetPlayerCamera(CameraPosition);

    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_SetPlayerCamera(Vector3 targetPosition)
    {
        mainCamera.transform.position = targetPosition;
    }
}
