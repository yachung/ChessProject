using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ShopPresenter
{
    private ShopView view;
    private ShopModel model;
    private ChampionManager championManager;

    [Inject] 
    public void Constructor(ShopView shopView, ShopModel model, ChampionManager championManager)
    {
        this.view = shopView;
        this.model = model;
        this.championManager = championManager;

        view.SetPresenter(this);
    }

    public void OnBuyChampion(ChampionData championData)
    {
        championManager.RPC_SummonChampion(championData.championPrefab, model.Runner.LocalPlayer);
    }
    
    public void OnRefreshShop()
    {
        view.SetChampionCards(model.GetRandomChampions());
    }

    public void OnBuyExperience()
    {
        RPC_BuyExperience();
    }

    public void OnSellChampion()
    {

    }

    public void UpdateView()
    {
        view.ShowUI();
        OnRefreshShop();
    }

    public void HideView()
    {
        view.HideUI();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_BuyExperience()
    {

    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //public void RPC_RefreshShop()
}
