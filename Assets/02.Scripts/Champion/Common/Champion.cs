using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public enum ChampionType
{
    Warrior,
    Magician,
    Archer,
}

public abstract class Champion : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(UpdateChampionStatus))] public ChampionStatus Status { get; set; }

    public ChampionController Controller { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<ChampionController>();
        Animator = GetComponentInChildren<Animator>();
        //Status = new ChampionStatus();
    }

    public void Initialize(ChampionStatus status)
    {
        this.Status = status;

        //_status.OnStatusChanged += UpdateChampionState;  // 상태 변경 이벤트를 구독

        // 초기 상태에 따른 세팅
        //UpdateChampionState(_status);

        UpdatePosition(status.ReadyCoord);  // 초기 배치 좌표로 이동
    }

    public void Damage(float attackPower)
    {
        //Status.HealthPoint -= attackPower;
    }

    public void UpdateChampionStatus()
    {
        Debug.Log(name);
        //// 상태 변화에 따라 챔피언의 동작을 업데이트
        //if (updatedStatus.IsDeath)
        //{
        //    // 사망 처리 로직
        //    Debug.Log($"{updatedStatus.Name} has died.");
        //    gameObject.SetActive(false);  // 유닛을 비활성화
        //}
        //else
        //{
        //    // 위치, 체력 등 갱신
        //    //transform.position = GetWorldPositionFromCoord(updatedStatus.BattleCoord);
        //}
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            if (Status.IsDrag)
            {
                transform.position = inputData.mousePosition;
            }
        }
    }

    public override void Spawned()
    {
        base.Spawned();
    }

    private void UpdatePosition(Vector2Int coord)
    {
        //GameManager.Instance.allPlayers[Object.InputAuthority].playerField.RPC_SetChampion(coord, this);

        Vector3 newPosition = GameManager.Instance.allPlayers[Object.InputAuthority].playerField.Tiles[coord.x, coord.y].DeployPoint;
        transform.position = newPosition;
    }
}
