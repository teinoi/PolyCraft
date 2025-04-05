using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// UI ��ư�� ���� �̵�/ȸ�� ��带 ����
/// </summary>
public class MeshTransform : MonoBehaviour
{
    // XRRayInteractor ����
    public XRRayInteractor rayInteractor;

    // Move ��ư Ŭ�� �� ȣ��(������Ʈ �̵� ���� ����)
    public void MoveButton()
    {
        if (rayInteractor != null)
        {
            rayInteractor.translateSpeed = 5f;  // �̵� �ӵ� ����
            rayInteractor.rotateSpeed = 0f;     // ȸ�� ��Ȱ��ȭ
        }

        RotationLimit[] rotationLimits = FindObjectsOfType<RotationLimit>();

        foreach (RotationLimit rotationLimit in rotationLimits)
        {
            rotationLimit.isRotate = false;
        }
    }

    // Rotate ��ư Ŭ�� �� ȣ��(������Ʈ ȸ�� ���� ����)
    public void RotateButton()
    {
        if (rayInteractor != null)
        {
            rayInteractor.translateSpeed = 0f;  // �̵� ��Ȱ��ȭ
            rayInteractor.rotateSpeed = 180f;   // ȸ�� �ӵ� ����
        }

        RotationLimit[] rotationLimits = FindObjectsOfType<RotationLimit>();

        foreach (RotationLimit rotationLimit in rotationLimits)
        {
            rotationLimit.isRotate = true;
        }
    }
}