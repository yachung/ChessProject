using Fusion;
using System;
using UnityEngine;
using VContainer;

/// <summary>
/// 실제 플레이어 캐릭터(오브젝트)
/// </summary>
public class Player : NetworkBehaviour
{
    [HideInInspector] public PlayerRef OwnerRef;
    
    // 플레이어 프로필 정보
    public PlayerInfo Info { get; private set; }

    /// <summary>
    /// 변화가 생기면 변화를 감지해서 UI를 수정해야함
    /// </summary>
    [Networked, OnChangedRender(nameof(OnLevelChangedRender))] public int Level { get; set; }
    [Networked, OnChangedRender(nameof(OnExpChangedRender))] public int Exp { get; set; }
    [Networked, OnChangedRender(nameof(OnGoldChangedRender))] public int Gold { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChangedRender))] public int Hp { get; set; }

    public Action<int> OnGoldChanged;
    public Action<int> OnExperienceChanged;
    public Action<int> OnLevelChanged;
    public Action<int> OnHpChanged;

    private PlayerController controller;
    private Camera mainCamera;

    // -------------------------------------------------------------
    // 라이프사이클
    // -------------------------------------------------------------
    public override void Spawned()
    {
        controller = GetComponent<PlayerController>();
        if (Object.HasInputAuthority)
        {
            // 플레이어 초기화 로직
            FindAnyObjectByType<ShopModel>().LocalPlayer = this;

            Level = 1;
            Hp = 100;
            Gold = 10;
            Exp = 0;
        }

        Debug.Log($"Player Spawned. OwnerRef={OwnerRef}, HP={Hp}");
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Debug.Log($"Player Despawned. OwnerRef={OwnerRef}");
    }

    // -------------------------------------------------------------
    // 설정/초기화 메서드
    // -------------------------------------------------------------
    /// <summary>
    /// Host(서버 권위)가 Spawn 직후 PlayerInfo를 세팅해줄 수 있음
    /// </summary>
    public void Initialize(PlayerInfo info)
    {
        Info = info;
        // 필요하면 Info의 닉네임 등을 UI에 표시할 수도
    }

    // -------------------------------------------------------------
    // OnChangedRender 콜백들 -> UI 업데이트 (InputAuthority만)
    // -------------------------------------------------------------
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

    public void PlayerTeleport(Vector3 position)
    {
        controller.PlayerTeleport(position);
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
