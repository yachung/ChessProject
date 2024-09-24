using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionCard : MonoBehaviour
{
    private Image Img_Champion;
    private TMP_Text Txt_ChampionName;
    private TMP_Text Txt_ChampionCost;

    public void Intialize(Champion champion)
    {
        Txt_ChampionName.text = champion.Name;
        Txt_ChampionCost.text = champion.Cost.ToString("N0");
    }
}
