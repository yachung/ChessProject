using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageModel : MonoBehaviour
{
    public int StageIndex { get; set; } = 0;
    public int RoundIndex { get; set; } = 0;
    public string StageName => $"{StageIndex}-{RoundIndex}";

}
