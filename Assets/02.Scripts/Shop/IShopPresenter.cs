using UnityEngine;

public interface IShopPresenter
{
    public void SetView(IShopView view);
    public void UpdateView();
    public void HideView();
    public void OnRefreshShop();
    public void OnBuyChampion(ChampionData championData);
    public void OnSellChampion();
    public void OnBuyExperience();
}
