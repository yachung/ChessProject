using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ShopPresenter : NetworkBehaviour
{
    [Inject] private readonly ShopView view;
    [Inject] private readonly ShopModel model;

    [Inject] private readonly ChampionManager championManager;

    public void OnBuyChampion(ChampionData championData)
    {
        championManager.RPC_SummonChampion(championData.championPrefab, Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<Player>());
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //public void RPC_RefreshShop()
    public void OnRefreshShop()
    {
        view.UpdateUI(model.GetRandomChampions());
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_BuyExperience()
    {

    }

    public void ShowUI()
    {
        view.ShowUI();
    }

    public void HideUI()
    {
        view.HideUI();
    }
}
