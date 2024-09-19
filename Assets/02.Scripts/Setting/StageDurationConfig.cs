using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default StageDurationConfig", menuName = "Stage/Stage Duration Config")]
public class StageDurationConfig : ScriptableObject
{
    public float 
        selectObjectDuration = 10f, 
        battleReadyDuration = 10f, 
        battleDuration = 10f, 
        winDuration = 0f;
}
