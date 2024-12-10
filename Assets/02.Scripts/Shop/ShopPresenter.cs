using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using WebSocketSharp;

public class ShopPresenter
{
    private ShopView shopView;
    private ShopModel shopModel;
    private ChampionSpawner championManager;

    [Inject] 
    public void Constructor(ShopView shopView, ShopModel model, ChampionSpawner championManager)
    {
        this.shopView = shopView;
        this.shopModel = model;
        this.championManager = championManager;

        shopModel.OnLocalPlayerChanged += SubscribeToDataEvents;

        this.shopView.SetPresenter(this);
    }

    public void SubscribeToDataEvents()
    {
        shopModel.LocalPlayer.OnGoldChanged += UpdateGoldView;
        shopModel.LocalPlayer.OnExperienceChanged += UpdateExperienceView;
        shopModel.LocalPlayer.OnLevelChanged += UpdateLevelView;
    }

    public void OnBuyChampion(ChampionData championData, Action<bool> OnCheckedGold)
    {
        bool IsPass = shopModel.LocalPlayer.Gold >= championData.cost;

        Debug.Log($"챔피언 구매 : {IsPass}, 잔돈 : {shopModel.LocalPlayer.Gold}");

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

    public void UpdateGoldView(int gold) 
    {
        shopView.UpdateGold(gold);
    }

    public void UpdateLevelView(int level)
    {
        shopView.UpdateLevel(level);
    }

    public void UpdateExperienceView(int exp)
    {
        shopView.UpdateExp(exp);
    }

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
