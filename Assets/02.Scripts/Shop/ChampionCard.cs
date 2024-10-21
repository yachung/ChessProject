using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionCard : MonoBehaviour
{
    [SerializeField] private Image Img_Champion;
    [SerializeField] private TMP_Text Txt_ChampionName;
    [SerializeField] private TMP_Text Txt_ChampionCost;
    [SerializeField] private Button Btn_BuyCard;

    private ChampionData championData;

    public void Initialize(Func<ChampionData, bool> callBack)
    {
        Btn_BuyCard.onClick.AddListener(() =>
        {
            bool isPass = callBack(championData);

            if (isPass)
                gameObject.SetActive(false);
        });
    }

    public void SetData(ChampionData championData)
    {
        this.championData = championData;

        Img_Champion.sprite = championData.cardImage;
        Txt_ChampionName.text = championData.championName;
        Txt_ChampionCost.text = championData.cost.ToString("N0");

        gameObject.SetActive(true);
    }

    public void OnClickBuyChampion()
    {

    }
}
