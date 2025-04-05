using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

/// <summary>
/// Ʈ���� �Է� �� Ray�� �浹�� ������Ʈ�� Outline�� Ȱ��ȭ�ϰ�, ���� ����� ����
/// </summary>
public class OutlineManager : MonoBehaviour
{
    public InputActionReference triggerAction;
    public XRRayInteractor rayInteractor;

    List<Transform> outlinedObjects = new List<Transform>();

    void OnEnable()
    {
        triggerAction.action.started += OnTriggerPressed;
        triggerAction.action.canceled += OnTriggerReleased;
    }

    void OnDisable()
    {
        triggerAction.action.started -= OnTriggerPressed;
        triggerAction.action.canceled -= OnTriggerReleased;
    }

    // Ʈ���Ÿ� ������ ��, Ray�� �浹�� ������Ʈ�� Outline�� Ȱ��ȭ
    void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Outline hitOutline = hit.transform.GetComponent<Outline>();

            if (hitOutline != null)
            {
                hitOutline.enabled = true;

                if (!outlinedObjects.Contains(hit.transform))
                {
                    outlinedObjects.Add(hit.transform);
                }
            }
            else
            {
                DisableAllOutlines();
            }
        }
        else
        {
            DisableAllOutlines();
        }
    }

    void OnTriggerReleased(InputAction.CallbackContext context)
    {
        // �߰� �۾��� �ʿ����� ����
    }

    // ��� Outline�� ��Ȱ��ȭ�ϰ� ����Ʈ �ʱ�ȭ
    void DisableAllOutlines()
    {
        // ��ȿ���� ���� ���� ����
        outlinedObjects.RemoveAll(obj => obj == null);

        foreach (Transform obj in outlinedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }

        // ����Ʈ �ʱ�ȭ
        outlinedObjects.Clear();
    }

    // ���� Outline�� ����� ��� ������Ʈ ����Ʈ ��ȯ
    public List<Transform> GetOutlinedObjects()
    {
        // ��ȿ�� ������ �����ϵ��� ���͸�
        outlinedObjects.RemoveAll(obj => obj == null);
        return outlinedObjects;
    }

    // Ư�� ������Ʈ�� Outline ��Ȱ��ȭ �� ����Ʈ���� ����
    public void RemoveOutline(Transform obj)
    {
        if (obj != null)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }

            outlinedObjects.Remove(obj);
        }
    }
}