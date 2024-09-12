using Fusion;
using TMPro;
using UnityEngine;

public class StateView : MonoBehaviour
{
    [SerializeField] TMP_Text txt_RemainTime;

    [Networked, OnChangedRender(nameof(RemainTimeChanged))] float remainTime { get; set; }

    private void RemainTimeChanged()
    {
        txt_RemainTime.text = remainTime.ToString();
    }
}
