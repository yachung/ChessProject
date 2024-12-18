using Fusion;
using UnityEngine;

public class ChampionDragAndDrop : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private PlayerField playerField;

    private Champion selectedChampion = null;
    private Vector3 originPosition;
    bool isDrag = false;

    private void Awake()
    {
        playerField = GetComponentInParent<PlayerField>();
        playerField.OnIsBattleChanged += OnBattleStart;
    }

    private void OnBattleStart(bool IsBattle)
    {
        if (IsBattle)
        {
            isDrag = false;

            //if (selectedChampion != null)
            //    selectedChampion.transform.position = originPosition;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            ProcessInput(inputData);
        }
    }

    private void ProcessInput(NetworkInputData inputData)
    {
        if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isDrag))
        {
            Debug.Log($"isDrag WasPressed : {inputData.movePosition}");
            originPosition = inputData.movePosition;

            isDrag = playerField.IsOccupied(originPosition, out selectedChampion);
        }

        if (inputData.buttons.WasReleased(ButtonsPrevious, MyButtons.isDrag) && isDrag)
        {
            Debug.Log($"isDrag WasReleased : {inputData.movePosition}");

            isDrag = false;
            if (selectedChampion != null)
            {
                playerField.UpdatePositionOnHost(originPosition, inputData.movePosition);
            }
            //OnMove(inputData.movePosition);
        }

        if (selectedChampion != null && isDrag)
        {
            selectedChampion.transform.position = inputData.movePosition;
        }

        ButtonsPrevious = inputData.buttons;
    }
}
