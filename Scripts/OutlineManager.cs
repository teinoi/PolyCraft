using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

/// <summary>
/// 트리거 입력 시 Ray에 충돌한 오브젝트의 Outline을 활성화하고, 선택 목록을 관리
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

    // 트리거를 눌렀을 때, Ray에 충돌한 오브젝트의 Outline을 활성화
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
        // 추가 작업이 필요하지 않음
    }

    // 모든 Outline을 비활성화하고 리스트 초기화
    void DisableAllOutlines()
    {
        // 유효하지 않은 참조 제거
        outlinedObjects.RemoveAll(obj => obj == null);

        foreach (Transform obj in outlinedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }

        // 리스트 초기화
        outlinedObjects.Clear();
    }

    // 현재 Outline이 적용된 모든 오브젝트 리스트 반환
    public List<Transform> GetOutlinedObjects()
    {
        // 유효한 참조만 유지하도록 필터링
        outlinedObjects.RemoveAll(obj => obj == null);
        return outlinedObjects;
    }

    // 특정 오브젝트의 Outline 비활성화 및 리스트에서 제거
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