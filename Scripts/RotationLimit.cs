using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// XRGrabInteractable�� ��ӹ޾� ������Ʈ�� ȸ�� ���� ���θ� ����
/// </summary>
public class RotationLimit : XRGrabInteractable
{
    // ������Ʈ�� ����� ���� �ʱ� ȸ���� ����
    Quaternion initialRotation;

    // ȸ�� ��� ����
    public bool isRotate = false;

    // ������Ʈ�� ����� �� ȣ��
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        // ȸ���� ����
        initialRotation = transform.rotation;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // ������Ʈ�� ���� �ְ� ȸ���� ������ �ʾ��� ��, �ʱ� ȸ�������� ����
        if (isSelected && !isRotate)
        {
            transform.rotation = initialRotation;
        }

        base.ProcessInteractable(updatePhase);

        // ȸ���� ������ �ʾ��� ���� ȸ���� ����
        if (isSelected && !isRotate)
        {
            Vector3 eulerRotation = transform.rotation.eulerAngles;

            // �ʱ� ȸ�������� ����
            transform.rotation = initialRotation;
        }
    }
}