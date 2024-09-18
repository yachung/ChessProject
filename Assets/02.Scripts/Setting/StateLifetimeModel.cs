using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default StateLifetime", menuName = "State/LifetimeModel")]
public class StateLifetimeModel : ScriptableObject
{
    public float 
        pregameDuration, 
        selectObjectDuration, 
        battleReadyDuration, 
        battleDuration, 
        winDuration;
}
