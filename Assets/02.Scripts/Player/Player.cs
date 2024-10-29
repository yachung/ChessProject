using Fusion;
using System;
using UnityEngine;
using VContainer;

    
public class Player : NetworkBehaviour
{
    private ShopPresenter shopPresenter;

    [Inject]
    public void Construct(ShopPresenter shopPresenter)
    {
        this.shopPresenter = shopPresenter;
    }

    //public PlayerInfo playerInfo { get; set; }
    [Networked] public PlayerField playerField { get; set; }

    private PlayerController playerController;
    private Camera mainCamera;

    private PlayerData playerData;

    public NetworkString<_32> Name => this.gameObject.name;

    [Networked, OnChangedRender(nameof(OnLevelChangedRender))] public int Level { get; set; }
    [Networked, OnChangedRender(nameof(OnExpChangedRender))] public int Exp { get; set; }
    [Networked, OnChangedRender(nameof(OnGoldChangedRender))] public int Gold { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChangedRender))] public int Hp { get; set; }


    public void OnGoldChangedRender()
    {
        playerData.OnGoldChanged?.Invoke(Gold);
    }
    public void OnLevelChangedRender()
    {
        playerData.OnLevelChanged?.Invoke(Level);
    }
    public void OnHpChangedRender()
    {
        playerData.OnHealthChanged?.Invoke(Hp);
    }
    public void OnExpChangedRender()
    {
        playerData.OnExperienceChanged?.Invoke(Exp);
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInChildren<PlayerController>();
    }

    public override void Spawned()
    {
        
    }

    public void SpawnedComplete()
    {
        if (Runner.IsServer)
        {
            RPC_PlayerInitialize(this.playerField);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_PlayerInitialize(PlayerField localField)
    {
        this.playerField = localField;
        GameManager.Instance.LocalPlayer = this;
        shopPresenter.PlayerInitialize();
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
