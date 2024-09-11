using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Input")]
    [SerializeField] private InputActionAsset playerInputActions;

    private InputAction moveInput;
    private InputAction dragInput;
    private InputAction pointInput;
    private InputAction refreshInput;
    private InputAction addExpInput;

    private void Awake()
    {
        moveInput = playerInputActions.FindAction("Move");
        dragInput = playerInputActions.FindAction("Drag");
        pointInput = playerInputActions.FindAction("Point");
        refreshInput = playerInputActions.FindAction("Refresh");
        addExpInput = playerInputActions.FindAction("AddExp");
    }

    private void OnEnable()
    {
        moveInput.Enable();
        dragInput.Enable();
        pointInput.Enable();
        refreshInput.Enable();
        addExpInput.Enable();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        inputData.buttons.Set(MyButtons.isMove, moveInput.IsPressed());
        inputData.buttons.Set(MyButtons.isDrag, dragInput.IsPressed());
        inputData.buttons.Set(MyButtons.isRefresh, refreshInput.IsPressed());
        inputData.buttons.Set(MyButtons.isAddExp, addExpInput.IsPressed());

        inputData.movePosition = pointInput.ReadValue<Vector2>();

        input.Set(inputData);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
