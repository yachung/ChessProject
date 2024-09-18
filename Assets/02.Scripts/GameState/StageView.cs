using Michsky.MUIP;
using UnityEngine;
using TMPro;

public class StageView : MonoBehaviour
{
    [SerializeField] public ProgressBar progressBar;
    [SerializeField] public bool showProgress = true;
    [SerializeField] public TMP_Text txt_CurrentStage;

    public void DisplayStageName(string stageName)
    {
        if (!string.IsNullOrEmpty(stageName))
            return;
        
        txt_CurrentStage.text = stageName;
    }

    public void UpdateProgressBar(float duration)
    {
        progressBar.SetValue(duration);
    }
}
