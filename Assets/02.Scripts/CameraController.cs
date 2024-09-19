using Fusion;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public Camera playerCamera; // 플레이어 카메라
    public Transform playerTransform; // 플레이어 위치 기준



    public override void Spawned()
    {
        // 로컬 플레이어인 경우에만 카메라 위치를 변경
        if (Object.HasInputAuthority)
        {
            // 카메라 초기 위치 설정
            SetCameraPosition();
        }
    }

    private void SetCameraPosition()
    {
        // 카메라를 플레이어의 위치나 특정 위치로 설정
        playerCamera.transform.position = playerTransform.position + new Vector3(0, 5, -10); // 예시: 플레이어 뒤쪽에서 내려다보는 카메라
        playerCamera.transform.LookAt(playerTransform); // 플레이어를 바라보도록 설정
    }

    public override void FixedUpdateNetwork()
    {
        // 로컬 플레이어 카메라 업데이트
        if (Object.HasInputAuthority)
        {
            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        // 여기서 카메라 위치를 지속적으로 플레이어에 맞춰 업데이트하거나,
        // 추가적인 카메라 조작 로직을 구현할 수 있습니다.
        playerCamera.transform.position = playerTransform.position + new Vector3(0, 5, -10);
        playerCamera.transform.LookAt(playerTransform);
    }
}
