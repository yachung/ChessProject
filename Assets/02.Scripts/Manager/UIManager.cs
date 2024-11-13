using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject ObjToastMessage;
    [SerializeField] private TMP_Text ToastMessage;

    public void ShowLoading(bool isActive)
    {
        LoadingPanel.SetActive(isActive);
    }

    public async void ShowMessage(string message)
    {
        ObjToastMessage.SetActive(true);
        ToastMessage.text = message;

        await UniTask.WaitForSeconds(3f);

        ObjToastMessage.SetActive(false);
    }
}
