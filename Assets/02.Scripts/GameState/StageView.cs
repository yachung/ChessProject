using Michsky.MUIP;
using UnityEngine;
using TMPro;

public class StageView : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text txt_CurrentStage;
    //[SerializeField] private bool showProgress = true;

    private Transform refTransform;

    private void Awake()
    {
        //refTransform = transform;
    }

    public void DisplayStageName(string stageName)
    {
        txt_CurrentStage.text = stageName;
    }

    public void UpdateProgressBar(float duration)
    {
        progressBar.SetValue(duration * 100);
    }

    public void ShowUI()
    {
        transform.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        transform.gameObject.SetActive(false);
    }
}
