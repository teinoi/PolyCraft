using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// XRGrabInteractable을 상속받아 오브젝트의 회전 가능 여부를 제어
/// </summary>
public class RotationLimit : XRGrabInteractable
{
    // 오브젝트를 잡았을 때의 초기 회전값 저장
    Quaternion initialRotation;

    // 회전 허용 여부
    public bool isRotate = false;

    // 오브젝트를 잡았을 때 호출
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        // 회전값 저장
        initialRotation = transform.rotation;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // 오브젝트가 잡혀 있고 회전이 허용되지 않았을 때, 초기 회전값으로 설정
        if (isSelected && !isRotate)
        {
            transform.rotation = initialRotation;
        }

        base.ProcessInteractable(updatePhase);

        // 회전이 허용되지 않았을 때는 회전을 고정
        if (isSelected && !isRotate)
        {
            Vector3 eulerRotation = transform.rotation.eulerAngles;

            // 초기 회전값으로 설정
            transform.rotation = initialRotation;
        }
    }
}