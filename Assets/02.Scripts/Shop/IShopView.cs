using System.Collections.Generic;
using UnityEngine;

public interface IShopView
{
    public void SetChampionCards(List<ChampionData> datas);
    public void ShowUI();
    public void HideUI();
}
