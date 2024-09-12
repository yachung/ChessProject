using Fusion;
using UnityEngine;
using VContainer;

public class Player : NetworkBehaviour
{
    private PlayerInfo playerInfo;
    private NetworkTransform controller;
    public PlayerField playerField { get; private set; }
    //private InputManager inputManager;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }


    private void Awake()
    {
        //inputManager = GetComponentInChildren<InputManager>();
        controller = GetComponent<NetworkTransform>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            //Runner.AddCallbacks(inputManager);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_PlayerFieldInitialize(PlayerField localField)
    {
        playerField = localField;
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
        controller.Teleport(destination);

        Debug.Log("OnMove");
    }

    public void MoveToPlayerField()
    {
        controller.Teleport(playerField.transform.position);
    }

    public void PlayerTeleport(Vector3 position)
    {
        Debug.Log($"Teleport is {name}: {position}");

        controller.Teleport(position);
    }
}
