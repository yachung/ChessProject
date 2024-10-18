using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SelectField : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    public Pose DefaultCameraData;

    private void Awake()
    {
        DefaultCameraData = new Pose(Camera.main.transform.position, Camera.main.transform.rotation);
    }

    public void SetPlayerPosition(Player[] players)
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (i >= players.Length)
                return;

            players[i].MoveToSelectField(spawnPositions[i].position, DefaultCameraData);
        }
    }
}
