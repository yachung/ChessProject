using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private PlayerInfo playerInfo;
    private NetworkTransform controller;
    private PlayerField playerField;

    private void Awake()
    {
        controller = GetComponent<NetworkTransform>();
    }

    public void Initialize(PlayerField playerField)
    {

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
