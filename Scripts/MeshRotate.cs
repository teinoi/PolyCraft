using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// ��Ʈ�ѷ��� ���̽�ƽ���� Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� ȸ���� ����
/// </summary>
public class MeshRotate : MonoBehaviour
{
    // ���� ��Ʈ�ѷ� ���̽�ƽ
    public InputActionReference leftRotateAction;

    // ������ ��Ʈ�ѷ� ���̽�ƽ
    public InputActionReference rightRotateAction;

    // OutlineManager ����
    public OutlineManager outlineManager;

    // ĳ���� ���� ��ũ��Ʈ ����
    public MonoBehaviour[] characterController;

    // ������Ʈ ȸ�� �ӵ�
    public float speed;

    // ȸ���� ������Ʈ ����Ʈ
    List<Transform> targetObjects = new List<Transform>();

    // ȸ�� ����
    bool isRotating = false;

    // ���� ȸ�� ��
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
        // Outline�� ��Ȱ��ȭ�Ǿ� ������ ȸ�� ����
        if (isRotating && outlineManager.GetOutlinedObjects().Count == 0)
        {
            StopRotating();
        }
    }

    // ��Ʈ�ѷ� �Է� �� ������Ʈ�� ȸ��
    void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (isRotating && targetObjects.Count > 0)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 rotation = Vector3.zero;

            // �� ���⿡ ���� ȸ�� ���� ����
            switch (rotateAxis)
            {
                case Axis.X:
                    rotation = new Vector3(input.x, 0, 0);  // X�� ȸ��
                    break;
                case Axis.Y:
                    rotation = new Vector3(0, input.x, 0);  // Y�� ȸ��
                    break;
                case Axis.Z:
                    rotation = new Vector3(0, 0, input.x);  // Z�� ȸ��
                    break;
            }

            // ��ȿ�� ������Ʈ�� ���͸� �� ȸ��
            targetObjects.RemoveAll(obj => obj == null);
            foreach (Transform obj in targetObjects)
            {
                if (obj != null)
                {
                    // ���� ��ǥ�� ���� ȸ��
                    obj.Rotate(rotation * Time.deltaTime * speed, Space.World);
                }
            }

            // Outline�� ��Ȱ��ȭ�Ǿ� ������ ȸ�� ����
            if (outlineManager.GetOutlinedObjects().Count == 0)
            {
                StopRotating();
            }
        }
    }

    // ȸ�� ����
    public void StartRotating(Axis axis)
    {
        rotateAxis = axis;

        // OutlineManager���� Outline Ȱ��ȭ ��ü ����Ʈ ��������
        targetObjects = outlineManager.GetOutlinedObjects();

        if (targetObjects.Count > 0)
        {
            isRotating = true;

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
    public void StopRotating()
    {
        isRotating = false;

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