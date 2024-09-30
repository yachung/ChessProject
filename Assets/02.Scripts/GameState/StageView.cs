using Michsky.MUIP;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class StageView : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text txt_CurrentStage;

    private StagePresenter stagePresenter;

    private ListViewItem[] playerList;
    //[SerializeField] private bool showProgress = true;

    private Transform refTransform;

    private void Awake()
    {
        playerList = GetComponentsInChildren<ListViewItem>();
    }

    public void SetPresenter(StagePresenter presenter)
    {
        this.stagePresenter = presenter;
    }

    public void DisplayStageName(string stageName)
    {
        txt_CurrentStage.text = stageName;
    }

    public void UpdateProgressBar(float duration)
    {
        progressBar.SetValue(duration * 100);
    }

    public void UpdatePlayerList(List<Player> players)
    {
        players = players.OrderBy(p => p.playerInfo.Hp).ToList();

        for (int i = 0; i < players.Count; ++i)
        {
            playerList[i].row0.textObject.text = players[i].playerInfo.Name.ToString();
            playerList[i].row1.textObject.text = players[i].playerInfo.Hp.ToString();
            playerList[i].button.onClick.AddListener(() => stagePresenter.OnClickPlayerList(players[i]));
        }
    }

    public void ShowUI()
    {
        transform.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        transform.gameObject.SetActive(false);
    }

    /// 플레이어는 이전에 만난 플레이어를 다시 만나지 않는다.
    /// 더 이상 매칭될 수 있는 플레이어가 없는 경우, 이전에 만난 플레이어를 초기화 한다.
    /// 플레이어의 수가 홀수인 경우 랜덤으로 매칭한다.
}
