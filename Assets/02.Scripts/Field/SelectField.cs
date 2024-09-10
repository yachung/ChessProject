using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SelectField : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;

    public void SetPlayerPosition(Player[] players)
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (i >= players.Length)
                return;

            players[i].PlayerTeleport(spawnPositions[i].position);
        }
    }
}
