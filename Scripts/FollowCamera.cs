using UnityEngine;

/// <summary>
/// UI ��ġ�� ī�޶� ��ġ�� ����ȭ
/// </summary>
public class FollowCamera : MonoBehaviour
{
    public Transform uiTransform;       // UI�� Transform
    public Transform cameraTransform;   // ī�޶��� Transform
    public float fixedYPosition = 20f;  // ������ Y�� ��ġ
    public float fixedZPosition = 10f;  // ������ Y�� ��ġ

    void Update()
    {
        // UI�� ��ġ�� ī�޶��� X, Z�࿡ ���߰�, Y���� ������ ������ ����
        Vector3 newPosition = new Vector3(cameraTransform.position.x, fixedYPosition, cameraTransform.position.z + fixedZPosition);
        uiTransform.position = newPosition;
    }
}
