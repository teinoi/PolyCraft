using UnityEngine;

/// <summary>
/// UI 위치를 카메라 위치에 동기화
/// </summary>
public class FollowCamera : MonoBehaviour
{
    public Transform uiTransform;       // UI의 Transform
    public Transform cameraTransform;   // 카메라의 Transform
    public float fixedYPosition = 20f;  // 고정된 Y축 위치
    public float fixedZPosition = 10f;  // 고정된 Y축 위치

    void Update()
    {
        // UI의 위치를 카메라의 X, Z축에 맞추고, Y축은 고정된 값으로 설정
        Vector3 newPosition = new Vector3(cameraTransform.position.x, fixedYPosition, cameraTransform.position.z + fixedZPosition);
        uiTransform.position = newPosition;
    }
}
