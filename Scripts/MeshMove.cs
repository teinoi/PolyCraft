using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// 컨트롤러의 조이스틱으로 Outline이 활성화되어 있는 오브젝트의 이동을 조작
/// </summary>
public class MeshMove : MonoBehaviour
{
    // 왼쪽 컨트롤러 조이스틱
    public InputActionReference leftMoveAction;

    // 오른쪽 컨트롤러 조이스틱
    public InputActionReference rightMoveAction;

    // OutlineManager 참조
    public OutlineManager outlineManager;

    // 캐릭터 조작 스크립트 참조
    public MonoBehaviour[] characterController;

    // 오브젝트 이동 속도
    public float speed;

    // 이동할 오브젝트 리스트
    List<Transform> targetObjects = new List<Transform>();

    // 이동 상태
    bool isMoving = false;

    // 현재 이동 축
    Axis moveAxis;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    void OnEnable()
    {
        leftMoveAction.action.performed += OnMovePerformed;
        rightMoveAction.action.performed += OnMovePerformed;
    }

    void OnDisable()
    {
        leftMoveAction.action.performed -= OnMovePerformed;
        rightMoveAction.action.performed -= OnMovePerformed;
    }

    void Update()
    {
        // Outline이 비활성화되어 있으면 이동 중지
        if (isMoving && outlineManager.GetOutlinedObjects().Count == 0)
        {
            StopMoving();
        }
    }

    // 컨트롤러 입력 시 오브젝트를 이동
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isMoving && targetObjects.Count > 0)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 movement = Vector3.zero;

            // 축 방향에 따라 이동 벡터 결정
            switch (moveAxis)
            {
                case Axis.X:
                    movement = new Vector3(input.x, 0, 0);  // X축 이동
                    break;
                case Axis.Y:
                    movement = new Vector3(0, input.x, 0);  // Y축 이동
                    break;
                case Axis.Z:
                    movement = new Vector3(0, 0, input.x);  // Z축 이동
                    break;
            }

            // 유효한 오브젝트만 필터링 후 이동
            targetObjects.RemoveAll(obj => obj == null);
            foreach (Transform obj in targetObjects)
            {
                if (obj != null)
                {
                    // 월드 좌표계 기준 이동
                    obj.Translate(movement * Time.deltaTime * speed, Space.World);
                }
            }

            // Outline이 비활성화되어 있으면 이동 중지
            if (outlineManager.GetOutlinedObjects().Count == 0)
            {
                StopMoving();
            }
        }
    }

    // 이동 시작
    public void StartMoving(Axis axis)
    {
        moveAxis = axis;

        // OutlineManager에서 Outline 활성화 객체 리스트 가져오기
        targetObjects = outlineManager.GetOutlinedObjects();

        if (targetObjects.Count > 0)
        {
            isMoving = true;

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
    public void StopMoving()
    {
        isMoving = false;

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