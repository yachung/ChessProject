using Fusion;
using System;
using UnityEngine;
using VContainer;

    
public class Player : NetworkBehaviour
{
    //public PlayerInfo playerInfo { get; set; }
    [Networked] public PlayerField playerField { get; set; }

    private PlayerController playerController;
    private Camera mainCamera;

    public NetworkString<_32> Name => this.gameObject.name;
    [Networked, OnChangedRender(nameof(OnLevelChangedRender))] public int Level { get; set; } = 1;
    [Networked, OnChangedRender(nameof(OnExpChangedRender))] public int Exp { get; set; } = 0;
    [Networked, OnChangedRender(nameof(OnGoldChangedRender))] public int Gold { get; set; } = 3;
    [Networked, OnChangedRender(nameof(OnHpChangedRender))] public int Hp { get; set; } = 100;

    public event Action<int> OnGoldChanged;
    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnHealthChanged;

    public void OnGoldChangedRender()
    {
        OnGoldChanged?.Invoke(Gold);
    }
    public void OnLevelChangedRender()
    {
        OnLevelChanged?.Invoke(Level);
    }
    public void OnHpChangedRender()
    {
        OnHealthChanged?.Invoke(Hp);
    }
    public void OnExpChangedRender()
    {
        OnExperienceChanged?.Invoke(Exp);
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInChildren<PlayerController>();
    }

    public override void Spawned()
    {
        // Todo: Spawn은 네트워크 Initialzie와 같은의미이며,
        // 여기서 값을 받아온다는건 서버에 요청을 보내서 받아온다는 의미.
        // 현재는 서버에서 플레이어를 생성함과 동시에 값을 보내주고있다.
        // networkmanager.instance.rpc_getplayerinformation


        // Runner.AddCallbacks(inputManager);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_PlayerInitialize(PlayerField localField)
    {
        this.playerField = localField;
        GameManager.Instance.LocalPlayer = this;
    }

    public void MoveToPlayerField(PlayerField playerField, bool isBattle = false)
    {
        PlayerTeleport(playerField.transform.position);

        if (isBattle)
            RPC_SetPlayerCamera(playerField.reverseCameraPose.position, playerField.reverseCameraPose.rotation);
        else
            RPC_SetPlayerCamera(playerField.cameraPose.position, playerField.cameraPose.rotation);
    }

    public void MoveToSelectField(Vector3 position, Pose cameraData)
    {
        PlayerTeleport(position);

        RPC_SetPlayerCamera(cameraData.position, cameraData.rotation);
    }

    public void PlayerTeleport(Vector3 position)
    {
        playerController.PlayerTeleport(position);
    }

    public void SetPlayerCamera(Pose transformData)
    {
        mainCamera.transform.SetPositionAndRotation(transformData.position, transformData.rotation);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_SetPlayerCamera(Vector3 position, Quaternion rotation)
    {
        mainCamera.transform.SetPositionAndRotation(position, rotation);
    }
}
