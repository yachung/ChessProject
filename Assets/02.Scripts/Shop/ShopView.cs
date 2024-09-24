using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    private TMP_Text txt_HaveCost;
    private TMP_Text txt_Level;
    private ProgressBar progressBar;
    private Button btn_BuyXp;
    private Button btn_Refresh;

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
