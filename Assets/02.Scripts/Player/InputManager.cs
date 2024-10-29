using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class InputManager : NetworkBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset playerInputActions;

    private Camera mainCamera;

    private InputAction moveInput;
    private InputAction dragInput;
    private InputAction pointInput;
    private InputAction refreshInput;
    private InputAction addExpInput;

    private void Awake()
    {
        mainCamera = FindAnyObjectByType<Camera>();

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

    public override void Spawned()
    {
        if (Runner.LocalPlayer == Object.InputAuthority)
        {
            var events = Runner.GetComponent<NetworkEvents>();

            events.OnInput.RemoveListener(OnInput);
            events.OnInput.AddListener(OnInput);
        }
    }

    public Vector3 InputPositionToWorldPosition(Vector3 inputPosition)
    {
        // 그리드가 위치한 평면을 정의 (y=height 평면)
        Plane gridPlane = new Plane(Vector3.up, this.transform.position);

        // 마우스 위치로부터 Ray 생성
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);

        // Ray와 평면의 교차점 계산
        if (gridPlane.Raycast(ray, out float distance))
        {
            // Ray와 평면이 교차하는 지점의 월드 좌표 반환
            return ray.GetPoint(distance);
        }

        return Vector3.zero;  // 실패한 경우 기본값 반환
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        inputData.buttons.Set(MyButtons.isMove, moveInput.IsPressed());
        inputData.buttons.Set(MyButtons.isDrag, dragInput.IsPressed());
        inputData.buttons.Set(MyButtons.isRefresh, refreshInput.IsPressed());
        inputData.buttons.Set(MyButtons.isAddExp, addExpInput.IsPressed());

        inputData.movePosition = InputPositionToWorldPosition(pointInput.ReadValue<Vector2>());

        input.Set(inputData);
    }
}
