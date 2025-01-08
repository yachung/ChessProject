using Fusion;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Host 모드에서, 플레이어와 필드 간의 "소유 관계" 및 "이동/매칭"을 일괄 관리
/// </summary>
public class FieldManager : NetworkBehaviour
{
    // 예: 최대 8명 지원
    // PlayerRef -> 필드 ID
    // (필드를 int ID로 구분; 실제 필드 객체는 로컬 배열 or lookup 통해 참조)
    [Networked, Capacity(8)]
    public NetworkDictionary<PlayerRef, int> AssignedFieldDict { get; set; }

    private PlayerField[] allFields;
    // 예: 0번 필드, 1번 필드, 2번 필드 ... (필드가 씬에 배치되어 있다고 가정)

    // PlayerManager, GameManager 등 다른 Manager 참조 (선택)
    //public PlayerManager playerManager;

    private void Awake()
    {
        allFields = FindObjectsByType<PlayerField>(FindObjectsSortMode.None);
    }

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            // 이벤트 등록해서 플레이어가 Join/Leave 시 필드를 할당/해제
            // var events = Runner.GetComponent<NetworkEvents>();
            // events.PlayerJoined.AddListener(OnPlayerJoined);
            // events.PlayerLeft.AddListener(OnPlayerLeft);
        }
    }

    /// <summary>
    /// 호스트(서버)가 플레이어에게 필드를 할당
    /// </summary>
    public void AssignFieldToPlayer(PlayerRef playerRef, int fieldId)
    {
        if (!Runner.IsServer) return;

        if (!AssignedFieldDict.ContainsKey(playerRef))
            AssignedFieldDict.Add(playerRef, fieldId);
        else
            AssignedFieldDict.Set(playerRef, fieldId);

        Debug.Log($"Assigned PlayerRef={playerRef} to FieldId={fieldId}");
    }

    /// <summary>
    /// 호스트(서버)가 "플레이어를 특정 필드로 이동" 명령
    /// </summary>
    public void MovePlayerToField(PlayerRef playerRef, bool isBattle)
    {
        if (!AssignedFieldDict.TryGet(playerRef, out int fieldId))
        {
            Debug.LogWarning($"{playerRef} has no assigned field.");
            return;
        }

        PlayerField fieldObj = GetFieldById(fieldId);
        if (!fieldObj)
        {
            Debug.LogWarning($"FieldId={fieldId} not found or not assigned in allFields.");
            return;
        }

        // Player 오브젝트 찾아서, PlayerController 이용해 이동
        Player player = FindPlayerObject(playerRef);
        if (!player)
        {
            Debug.LogWarning($"No Player object for {playerRef} found.");
            return;
        }

        player.controller.MoveToField(fieldObj, isBattle);
    }

    /// <summary>
    /// 로컬 배열(allFields)에서 fieldId에 해당하는 PlayerField 참조 반환
    /// </summary>
    private PlayerField GetFieldById(int fieldId)
    {
        if (fieldId >= 0 && fieldId < allFields.Length)
            return allFields[fieldId];
        return null;
    }

    /// <summary>
    /// 씬 상에서 해당 PlayerRef를 Owner로 갖는 Player 객체 찾기
    /// (실제 구현은 PlayerManager나 다른 방식을 사용할 수 있음)
    /// </summary>
    private Player FindPlayerObject(PlayerRef playerRef)
    {
        Player[] allPlayers = FindObjectsOfType<Player>();
        foreach (var p in allPlayers)
        {
            if (p.OwnerRef == playerRef)
                return p;
        }
        return null;
    }
}
