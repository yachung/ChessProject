using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ShopPresenter
{
    private ShopView shopView;
    private ShopModel shopModel;
    private ChampionManager championManager;

    [Inject] 
    public void Constructor(ShopView shopView, ShopModel model, ChampionManager championManager)
    {
        this.shopView = shopView;
        this.shopModel = model;
        this.championManager = championManager;

        this.shopView.SetPresenter(this);
    }

    public void OnBuyChampion(ChampionData championData, Action<bool> OnCheckedGold)
    {
        bool IsPass = shopModel.player.Gold >= championData.cost;

        Debug.Log($"챔피언 구매 : {IsPass}, 잔돈 : {shopModel.player.Gold}");

        OnCheckedGold?.Invoke(IsPass);

        if (IsPass)
            championManager.RPC_SummonChampion(championData.championName, shopModel.Runner.LocalPlayer);
    }
    
    public void OnRefreshShop()
    {
        shopView.SetChampionCards(shopModel.GetRandomChampions());
    }

    public void OnBuyExperience()
    {
        RPC_BuyExperience();
    }

    public void OnSellChampion()
    {

    }

    //public void UpdateView()
    //{
    //    shopView.ShowUI();
    //    OnRefreshShop();
    //}

    public void ShowView()
    {
        shopView.gameObject.SetActive(true);
    }

    public void HideView()
    {
        shopView.gameObject.SetActive(false);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_BuyExperience()
    {

    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //public void RPC_RefreshShop()
}
