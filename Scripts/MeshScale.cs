using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

/// <summary>
/// ��Ʈ�ѷ� �������� Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� ũ�⸦ ����
/// </summary>
public class MeshScale : MonoBehaviour
{
    // ��Ʈ�ѷ��� Ʈ���� �Է� ����
    public InputActionReference triggerAction;

    // OutlineManager ����
    public OutlineManager outlineManager;

    // XRRayInteractor ����
    public XRRayInteractor rayInteractor;

    // ĳ���� ���� ��ũ��Ʈ ����
    public MonoBehaviour[] characterController;

    // ���� ������ ��� ������Ʈ
    List<Transform> targetObjects = new List<Transform>();

    // �ʱ� ������ �� ����
    Vector3[] initialScales;

    // �����ϸ� ����
    bool isScaling = false;

    // �����ϸ� ��
    Axis scaleAxis = Axis.All;

    public enum Axis
    {
        X,
        Y,
        Z,
        All
    }

    void OnEnable()
    {
        triggerAction.action.performed += OnTriggerPerformed;
    }

    void OnDisable()
    {
        triggerAction.action.performed -= OnTriggerPerformed;
    }

    // Ʈ���� �Է� �� �����ϸ� ����
    void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        if (isScaling)
        {
            StopScaling();
        }
    }

    void Update()
    {
        // �����ϸ� �����̸� ���� ����� �ϳ� �̻� ���� ��
        if (isScaling && targetObjects.Count > 0)
        {
            if (outlineManager != null)
            {
                // ���õ� ��� ������Ʈ�� ���� �ݺ�
                for (int i = 0; i < targetObjects.Count; i++)
                {
                    Transform targetObject = targetObjects[i];

                    // ���� ������Ʈ�� Outline�� Ȱ��ȭ�Ǿ� �ִٸ�
                    if (outlineManager.GetOutlinedObjects().Contains(targetObject))
                    {
                        // XRRayInteractor�� Ray�� �浹�ߴ��� �˻�
                        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                        {
                            // Ray�� �浹�� ����
                            Vector3 hitPoint = hit.point;

                            // �ʱ� �������� �������� ����
                            Vector3 newScale = initialScales[i];

                            // ���õ� �࿡ ���� ������ ���(Ray�� �浹 ������ ��ü �߽� ���� �Ÿ� * 2)
                            switch (scaleAxis)
                            {
                                case Axis.X:
                                    newScale.x = Mathf.Abs(hitPoint.x - targetObject.position.x) * 2;
                                    break;
                                case Axis.Y:
                                    newScale.y = Mathf.Abs(hitPoint.y - targetObject.position.y) * 2;
                                    break;
                                case Axis.Z:
                                    newScale.z = Mathf.Abs(hitPoint.z - targetObject.position.z) * 2;
                                    break;
                                case Axis.All:
                                    float scaleMultiplier = Vector3.Distance(hitPoint, targetObject.position);
                                    newScale = initialScales[i] * scaleMultiplier;
                                    break;
                            }

                            // ���� �������� ������Ʈ�� ����
                            targetObject.localScale = newScale;
                        }
                    }
                    else
                    {
                        StopScaling();
                    }
                }
            }
        }
    }

    // �����ϸ� ����
    public void StartScaling(Axis axis)
    {
        scaleAxis = axis;

        // OutlineManager���� Outline Ȱ��ȭ ��ü ����Ʈ ��������
        var outlinedObjects = outlineManager?.GetOutlinedObjects();

        if (outlinedObjects != null && outlinedObjects.Count > 0)
        {
            targetObjects = new List<Transform>(outlinedObjects);
            initialScales = new Vector3[targetObjects.Count];

            // ��� ĳ���� ���� ��ũ��Ʈ ��Ȱ��ȭ
            for (int i = 0; i < targetObjects.Count; i++)
            {
                initialScales[i] = targetObjects[i].localScale;
            }

            isScaling = true;
        }
    }

    // �����ϸ� ����
    public void StopScaling()
    {
        isScaling = false;

        // ����Ʈ �ʱ�ȭ
        targetObjects.Clear();

        // ��� ĳ���� ���� ��ũ��Ʈ Ȱ��ȭ
        foreach (var controller in characterController)
        {
            if (controller != null)
            {
                controller.enabled = true;
            }
        }
    }
}