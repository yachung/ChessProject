using Fusion;
using UnityEngine;

public class ChampionDragHandler : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private PlayerField playerField;

    private ChampionStatus selectedChampion = default;
    private Vector3 originPosition;
    bool isDrag = false;

    private void Awake()
    {
        playerField = GetComponentInParent<PlayerField>();
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
            Debug.Log($"isDrag WasPressed : {inputData.mousePosition}");
            originPosition = inputData.mousePosition;

            isDrag = playerField.IsOccupied(originPosition, out selectedChampion);
            selectedChampion.IsDrag = isDrag;
        }

        if (inputData.buttons.WasReleased(ButtonsPrevious, MyButtons.isDrag))
        {
            Debug.Log($"isDrag WasReleased : {inputData.mousePosition}");

            if (isDrag)
            {
                playerField.UpdatePositionOnHost(originPosition, inputData.mousePosition);
            }

            isDrag = false;
            selectedChampion.IsDrag = isDrag;

            //OnMove(inputData.movePosition);
        }

        //if (selectedChampion != null && isDrag)
        //{
        //    selectedChampion.transform.position = inputData.movePosition;
        //}

        ButtonsPrevious = inputData.buttons;
    }
}
