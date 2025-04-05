using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// ��Ʈ�ѷ��� ���̽�ƽ���� Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� �̵��� ����
/// </summary>
public class MeshMove : MonoBehaviour
{
    // ���� ��Ʈ�ѷ� ���̽�ƽ
    public InputActionReference leftMoveAction;

    // ������ ��Ʈ�ѷ� ���̽�ƽ
    public InputActionReference rightMoveAction;

    // OutlineManager ����
    public OutlineManager outlineManager;

    // ĳ���� ���� ��ũ��Ʈ ����
    public MonoBehaviour[] characterController;

    // ������Ʈ �̵� �ӵ�
    public float speed;

    // �̵��� ������Ʈ ����Ʈ
    List<Transform> targetObjects = new List<Transform>();

    // �̵� ����
    bool isMoving = false;

    // ���� �̵� ��
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
        // Outline�� ��Ȱ��ȭ�Ǿ� ������ �̵� ����
        if (isMoving && outlineManager.GetOutlinedObjects().Count == 0)
        {
            StopMoving();
        }
    }

    // ��Ʈ�ѷ� �Է� �� ������Ʈ�� �̵�
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isMoving && targetObjects.Count > 0)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 movement = Vector3.zero;

            // �� ���⿡ ���� �̵� ���� ����
            switch (moveAxis)
            {
                case Axis.X:
                    movement = new Vector3(input.x, 0, 0);  // X�� �̵�
                    break;
                case Axis.Y:
                    movement = new Vector3(0, input.x, 0);  // Y�� �̵�
                    break;
                case Axis.Z:
                    movement = new Vector3(0, 0, input.x);  // Z�� �̵�
                    break;
            }

            // ��ȿ�� ������Ʈ�� ���͸� �� �̵�
            targetObjects.RemoveAll(obj => obj == null);
            foreach (Transform obj in targetObjects)
            {
                if (obj != null)
                {
                    // ���� ��ǥ�� ���� �̵�
                    obj.Translate(movement * Time.deltaTime * speed, Space.World);
                }
            }

            // Outline�� ��Ȱ��ȭ�Ǿ� ������ �̵� ����
            if (outlineManager.GetOutlinedObjects().Count == 0)
            {
                StopMoving();
            }
        }
    }

    // �̵� ����
    public void StartMoving(Axis axis)
    {
        moveAxis = axis;

        // OutlineManager���� Outline Ȱ��ȭ ��ü ����Ʈ ��������
        targetObjects = outlineManager.GetOutlinedObjects();

        if (targetObjects.Count > 0)
        {
            isMoving = true;

            // ��� ĳ���� ���� ��ũ��Ʈ ��Ȱ��ȭ
            foreach (var controller in characterController)
            {
                if (controller != null)
                {
                    controller.enabled = false;
                }
            }
        }
    }

    // �̵� ����
    public void StopMoving()
    {
        isMoving = false;

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