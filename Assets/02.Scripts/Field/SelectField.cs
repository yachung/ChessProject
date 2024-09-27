using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SelectField : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    public Vector3 DefaultCameraPosition = new Vector3(0, 45, -65);

    public void SetPlayerPosition(Player[] players)
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (i >= players.Length)
                return;

            players[i].PlayerTeleport(spawnPositions[i].position, DefaultCameraPosition);
        }
    }
}
