using UnityEngine;
using TMPro;
using Fusion;


public class PlayerInfoCell : MonoBehaviour
{
    [SerializeField] private TMP_Text playerIndex;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerHp;
    [SerializeField] private GameObject objHpContainer;


    public void Initialize(PlayerInfo player)
    {
        gameObject.SetActive(true);
        playerIndex.text = player.Index.ToString();
        playerName.text = player.Name.ToString();
        objHpContainer.SetActive(false);
    }

    public void Initialize(PlayerInfo player, int hp)
    {
        gameObject.SetActive(true);
        playerName.text = player.Name.ToString();
        playerHp.text = hp.ToString();
        objHpContainer.SetActive(true);
    }

    public void PlayerLeft()
    {
        gameObject.SetActive(false);
    }
}
