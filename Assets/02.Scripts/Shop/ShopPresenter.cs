using UnityEngine;
using VContainer;

public class ShopPresenter : MonoBehaviour
{
    [Inject] private readonly ShopView view;
    [Inject] private readonly ShopModel model;

    [Inject] private readonly ChampionManager championManager;

    public void ShowUI()
    {
        view.ShowUI();
    }

    public void HideUI()
    {
        view.HideUI();
    }
}
