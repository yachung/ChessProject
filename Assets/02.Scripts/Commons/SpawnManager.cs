using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SpawnManager : NetworkBehaviour
{
    [Inject] private readonly PlayerManager playerManager;
    [Inject] private readonly FieldManager fieldManager;
    [Inject] private readonly ChampionManager championManager;

    [SerializeField] private NetworkPrefabRef NetworkPlayerPref;


    public void PlayerSpawned()
    {
        if (!Runner.IsServer)
            return;

        int index = 0;
        foreach (var playerRef in Runner.ActivePlayers)
        {
            NetworkObject networkObject = Runner.Spawn(NetworkPlayerPref, Vector3.zero, Quaternion.identity, playerRef);

            fieldManager.AssignFieldToPlayer(playerRef, index++);

            Player player = networkObject.GetComponent<Player>();

            playerManager.AddPlayer(playerRef, player);
        }
    }
}
