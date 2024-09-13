using Fusion;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] TMP_Text txt_RemainTime;
    [SerializeField] public ProgressBar progressBar;

    float remainTime { get; set; }

    private void RemainTimeChanged()
    {
        txt_RemainTime.text = remainTime.ToString();
    }
}
