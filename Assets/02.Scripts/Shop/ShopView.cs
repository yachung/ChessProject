using Michsky.MUIP;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopView : MonoBehaviour
{
    [Inject] private readonly ShopPresenter _presenter;

    [SerializeField] private TMP_Text txt_HaveCost;
    [SerializeField] private TMP_Text txt_Level;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private Button btn_BuyXp;
    [SerializeField] private Button btn_Refresh;
    [SerializeField] private ChampionCard[] championCards;

    private void Start()
    {
        btn_Refresh.onClick.AddListener(() => OnClickRefresh());
        btn_BuyXp.onClick.AddListener(() => OnClickBuyXP());
        foreach (var card in championCards)
        {
            card.Initialize(OnClickBuyChampion);
        }
    }

    public void UpdateUI(List<ChampionData> datas)
    {
        for (int i = 0; i < championCards.Length; ++i)
        {
            if (i >= datas.Count)
                return;

            championCards[i].SetData(datas[i]);
        }
    }

    #region OnClickEvents
    private void OnClickRefresh()
    {
        _presenter.OnRefreshShop();
    }

    private void OnClickBuyXP()
    {
        _presenter.RPC_BuyExperience();
    }

    private void OnClickBuyChampion(ChampionData data)
    {
        _presenter.OnBuyChampion(data);
    }
    #endregion

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
