using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

/// <summary>
/// 컨트롤러 조작으로 Outline이 활성화되어 있는 오브젝트의 크기를 변경
/// </summary>
public class MeshScale : MonoBehaviour
{
    // 컨트롤러의 트리거 입력 참조
    public InputActionReference triggerAction;

    // OutlineManager 참조
    public OutlineManager outlineManager;

    // XRRayInteractor 참조
    public XRRayInteractor rayInteractor;

    // 캐릭터 조작 스크립트 참조
    public MonoBehaviour[] characterController;

    // 현재 스케일 대상 오브젝트
    List<Transform> targetObjects = new List<Transform>();

    // 초기 스케일 값 저장
    Vector3[] initialScales;

    // 스케일링 상태
    bool isScaling = false;

    // 스케일링 축
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

    // 트리거 입력 시 스케일링 종료
    void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        if (isScaling)
        {
            StopScaling();
        }
    }

    void Update()
    {
        // 스케일링 상태이며 조작 대상이 하나 이상 있을 때
        if (isScaling && targetObjects.Count > 0)
        {
            if (outlineManager != null)
            {
                // 선택된 모든 오브젝트에 대해 반복
                for (int i = 0; i < targetObjects.Count; i++)
                {
                    Transform targetObject = targetObjects[i];

                    // 현재 오브젝트가 Outline이 활성화되어 있다면
                    if (outlineManager.GetOutlinedObjects().Contains(targetObject))
                    {
                        // XRRayInteractor의 Ray가 충돌했는지 검사
                        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                        {
                            // Ray가 충돌한 지점
                            Vector3 hitPoint = hit.point;

                            // 초기 스케일을 기준으로 변경
                            Vector3 newScale = initialScales[i];

                            // 선택된 축에 따라 스케일 계산(Ray의 충돌 지점과 객체 중심 사이 거리 * 2)
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

                            // 계산된 스케일을 오브젝트에 적용
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

    // 스케일링 시작
    public void StartScaling(Axis axis)
    {
        scaleAxis = axis;

        // OutlineManager에서 Outline 활성화 객체 리스트 가져오기
        var outlinedObjects = outlineManager?.GetOutlinedObjects();

        if (outlinedObjects != null && outlinedObjects.Count > 0)
        {
            targetObjects = new List<Transform>(outlinedObjects);
            initialScales = new Vector3[targetObjects.Count];

            // 모든 캐릭터 조작 스크립트 비활성화
            for (int i = 0; i < targetObjects.Count; i++)
            {
                initialScales[i] = targetObjects[i].localScale;
            }

            isScaling = true;
        }
    }

    // 스케일링 종료
    public void StopScaling()
    {
        isScaling = false;

        // 리스트 초기화
        targetObjects.Clear();

        // 모든 캐릭터 조작 스크립트 활성화
        foreach (var controller in characterController)
        {
            if (controller != null)
            {
                controller.enabled = true;
            }
        }
    }
}