using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// 컨트롤러의 조이스틱으로 Outline이 활성화되어 있는 오브젝트의 회전을 조작
/// </summary>
public class MeshRotate : MonoBehaviour
{
    // 왼쪽 컨트롤러 조이스틱
    public InputActionReference leftRotateAction;

    // 오른쪽 컨트롤러 조이스틱
    public InputActionReference rightRotateAction;

    // OutlineManager 참조
    public OutlineManager outlineManager;

    // 캐릭터 조작 스크립트 참조
    public MonoBehaviour[] characterController;

    // 오브젝트 회전 속도
    public float speed;

    // 회전할 오브젝트 리스트
    List<Transform> targetObjects = new List<Transform>();

    // 회전 상태
    bool isRotating = false;

    // 현재 회전 축
    Axis rotateAxis;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    void OnEnable()
    {
        leftRotateAction.action.performed += OnRotatePerformed;
        rightRotateAction.action.performed += OnRotatePerformed;
    }

    void OnDisable()
    {
        leftRotateAction.action.performed -= OnRotatePerformed;
        rightRotateAction.action.performed -= OnRotatePerformed;
    }

    void Update()
    {
        // Outline이 비활성화되어 있으면 회전 중지
        if (isRotating && outlineManager.GetOutlinedObjects().Count == 0)
        {
            StopRotating();
        }
    }

    // 컨트롤러 입력 시 오브젝트를 회전
    void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (isRotating && targetObjects.Count > 0)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 rotation = Vector3.zero;

            // 축 방향에 따라 회전 벡터 결정
            switch (rotateAxis)
            {
                case Axis.X:
                    rotation = new Vector3(input.x, 0, 0);  // X축 회전
                    break;
                case Axis.Y:
                    rotation = new Vector3(0, input.x, 0);  // Y축 회전
                    break;
                case Axis.Z:
                    rotation = new Vector3(0, 0, input.x);  // Z축 회전
                    break;
            }

            // 유효한 오브젝트만 필터링 후 회전
            targetObjects.RemoveAll(obj => obj == null);
            foreach (Transform obj in targetObjects)
            {
                if (obj != null)
                {
                    // 월드 좌표계 기준 회전
                    obj.Rotate(rotation * Time.deltaTime * speed, Space.World);
                }
            }

            // Outline이 비활성화되어 있으면 회전 중지
            if (outlineManager.GetOutlinedObjects().Count == 0)
            {
                StopRotating();
            }
        }
    }

    // 회전 시작
    public void StartRotating(Axis axis)
    {
        rotateAxis = axis;

        // OutlineManager에서 Outline 활성화 객체 리스트 가져오기
        targetObjects = outlineManager.GetOutlinedObjects();

        if (targetObjects.Count > 0)
        {
            isRotating = true;

            // 모든 캐릭터 조작 스크립트 비활성화
            foreach (var controller in characterController)
            {
                if (controller != null)
                {
                    controller.enabled = false;
                }
            }
        }
    }

    // 이동 중지
    public void StopRotating()
    {
        isRotating = false;

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