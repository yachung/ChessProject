using Fusion;
using System;
using UnityEngine;
using VContainer;

    
public class Player : NetworkBehaviour, IAfterSpawned
{
    //private PlayerData playerData;
    //public PlayerData PlayerData => playerData;

    public NetworkString<_32> Name => Runner.name;

    /// <summary>
    /// 변화가 생기면 변화를 감지해서 UI를 수정해야함
    /// 
    /// </summary>

    [Networked, OnChangedRender(nameof(OnLevelChangedRender))] public int Level { get; set; }
    [Networked, OnChangedRender(nameof(OnExpChangedRender))] public int Exp { get; set; }
    [Networked, OnChangedRender(nameof(OnGoldChangedRender))] public int Gold { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChangedRender))] public int Hp { get; set; }

    public Action<int> OnGoldChanged;
    public Action<int> OnExperienceChanged;
    public Action<int> OnLevelChanged;
    public Action<int> OnHpChanged;

    [Networked] public PlayerField playerField { get; set; }

    private PlayerController playerController;
    private Camera mainCamera;


    public void OnGoldChangedRender()
    {
        if (HasInputAuthority)
            OnGoldChanged?.Invoke(Gold);
    }

    public void OnLevelChangedRender()
    {
        if (HasInputAuthority)
            OnLevelChanged?.Invoke(Level);
    }

    public void OnHpChangedRender()
    {
        if (HasInputAuthority)
            OnHpChanged?.Invoke(Hp);
    }

    public void OnExpChangedRender()
    {
        if (HasInputAuthority)
            OnExperienceChanged?.Invoke(Exp);
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = GetComponentInChildren<PlayerController>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
            FindAnyObjectByType<ShopModel>().LocalPlayer = this;
    }

    [Networked] private bool isInitialized { get; set; }

    public override void FixedUpdateNetwork()
    {
        //if (Runner.IsServer && !isInitialized)
        //{
        //    Level = 1;
        //    Exp = 0;
        //    Hp = 100;
        //    Gold = 10;

        //    isInitialized = true;
        //}
    }

    public void AfterSpawned()
    {
        if (Runner.IsServer)
        {

        }
    }

    public void SpawnedComplete()
    {
        if (Runner.IsServer)
        {
            RPC_PlayerInitialize(this.playerField);

            Level += 1;
            Hp += 100;
            Gold += 10;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_PlayerInitialize(PlayerField localField)
    {
        this.playerField = localField;
        //GameManager.Instance.LocalPlayer = this;
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
