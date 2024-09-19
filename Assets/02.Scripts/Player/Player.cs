using Fusion;
using UnityEngine;
using VContainer;

public class Player : NetworkBehaviour
{
    private PlayerInfo playerInfo;
    private NetworkTransform netTransform;
    public PlayerField playerField { get; private set; }
    private Camera mainCamera;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }


    private void Awake()
    {
        mainCamera = Camera.main;
        netTransform = GetComponent<NetworkTransform>();
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

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isMove))
            {
                Debug.Log($"isMove WasPressed : {inputData.movePosition}");
                OnMove(inputData.movePosition);
            }

            if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isDrag))
            {
                Debug.Log($"isDrag WasPressed : {inputData.movePosition}");
            }

            if (inputData.buttons.WasReleased(ButtonsPrevious, MyButtons.isDrag))
            {
                Debug.Log($"isDrag WasReleased : {inputData.movePosition}");
                //OnMove(inputData.movePosition);
            }

            if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isRefresh))
            {
                Debug.Log($"isRefresh WasPressed");
            }

            if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isAddExp))
            {
                Debug.Log($"isAddExp WasPressed");
            }

            ButtonsPrevious = inputData.buttons;
        }
    }

    private void OnMove(Vector3 destination)
    {
        netTransform.Teleport(destination);

        Debug.Log("OnMove");
    }

    public void MoveToPlayerField(PlayerField playerField)
    {
        PlayerTeleport(playerField.transform.position);
        RPC_PlayerTeleport(playerField);
        //mainCamera.transform.position = playerField.cameraPosition;
    }

    public void PlayerTeleport(Vector3 position)
    {
        if (Runner.IsServer)
        {
            Debug.Log($"Teleport is {name}: {position}");

            netTransform.Teleport(position);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_PlayerTeleport(PlayerField playerField)
    {
        //PlayerTeleport(playerField.transform.position);

        mainCamera.transform.position = playerField.cameraPosition;
    }
}
