using Fusion;
using UnityEngine;
using VContainer;

public class PlayerController : NetworkBehaviour
{
    public NetworkTransform networkTransform { get; private set; }
    public Animator Animator { get; private set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    public Vector3 Destination { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        networkTransform = GetComponentInParent<NetworkTransform>();
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
        if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isMove))
        {
            Debug.Log($"isMove WasPressed : {inputData.mousePosition}");
            Destination = inputData.mousePosition;
        }

        if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isRefresh))
        {
            Debug.Log($"isRefresh WasPressed");
        }

        if (inputData.buttons.WasPressed(ButtonsPrevious, MyButtons.isAddExp))
        {
            Debug.Log($"isAddExp WasPressed");
        }

        ButtonsPrevious = inputData.buttons;
    }

    public float MoveToward()
    {
        return MoveToward(transform, networkTransform, Destination, 50f);
    }

    public float MoveToward(Transform selfTransform, NetworkTransform networkTransform, Vector3 destination, float moveSpeed)
    {
        // 프레임 마다 이동을 처리한 뒤
        // 목적지까지 남은 거리를 전달
        // 이동 방향
        Vector3 direction = destination - selfTransform.position;

        // 이동하려는 목적지를 향해서 꾸준히 이동
        // 다음에 이동할 양(Amount)을 구하기 - 방향이 고려되어야 함
        Vector3 moveAmount = direction.normalized * moveSpeed * Runner.DeltaTime;
        //moveAmount.y = 0f;

        networkTransform.transform.position += moveAmount;

        // 도착했는지 확인 후에 도착했으면 Idle 스테이트로 전환
        return direction.magnitude;
    }

    public float CheckDistance()
    {
        Vector3 direction = Destination - transform.position;
        return direction.magnitude;
    }

    public void PlayerTeleport(Vector3 position)
    {
        Destination = position;

        if (Runner.IsServer)
        {
            networkTransform.Teleport(position);
        }
    }

    public override void Render()
    {

    }
}
