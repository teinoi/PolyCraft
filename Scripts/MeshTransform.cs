using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// UI 버튼을 통해 이동/회전 모드를 설정
/// </summary>
public class MeshTransform : MonoBehaviour
{
    // XRRayInteractor 참조
    public XRRayInteractor rayInteractor;

    // Move 버튼 클릭 시 호출(오브젝트 이동 모드로 설정)
    public void MoveButton()
    {
        if (rayInteractor != null)
        {
            rayInteractor.translateSpeed = 5f;  // 이동 속동 설정
            rayInteractor.rotateSpeed = 0f;     // 회전 비활성화
        }

        RotationLimit[] rotationLimits = FindObjectsOfType<RotationLimit>();

        foreach (RotationLimit rotationLimit in rotationLimits)
        {
            rotationLimit.isRotate = false;
        }
    }

    // Rotate 버튼 클릭 시 호출(오브젝트 회전 모드로 설정)
    public void RotateButton()
    {
        if (rayInteractor != null)
        {
            rayInteractor.translateSpeed = 0f;  // 이동 비활성화
            rayInteractor.rotateSpeed = 180f;   // 회전 속도 설정
        }

        RotationLimit[] rotationLimits = FindObjectsOfType<RotationLimit>();

        foreach (RotationLimit rotationLimit in rotationLimits)
        {
            rotationLimit.isRotate = true;
        }
    }
}