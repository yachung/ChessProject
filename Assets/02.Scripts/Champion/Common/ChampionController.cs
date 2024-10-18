using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionController : NetworkBehaviour
{
    [SerializeField] Image Img_RemainHp;
    [SerializeField] GameObject championUI;

    public Animator Animator { get; private set; }
    public Champion Target { get; private set; }

    public ChampionController (Animator animator)
    {
        this.Animator = animator;
    }

    public void OnHpChanged(float hpRatio)
    {
        if (hpRatio > 0)
            championUI.SetActive(true);
        else
            championUI.SetActive(false);

        Img_RemainHp.fillAmount = hpRatio;
    }

    public void HpColorChange(Color color)
    {
        Img_RemainHp.color = color;
    }

    // 탐색
    public bool EnemySearch(out Champion target)
    {
        target = Target;

        return false;
    }

    public bool InRangeCheck()
    {
        return false;
    }

    public bool IsAttackable(Champion target)
    {
        return false;
    }

    private bool IsDead(Champion target)
    {
        return false;
    }

    private void MoveNextTile()
    {

    }

    private void Attack()
    {

    }
}
