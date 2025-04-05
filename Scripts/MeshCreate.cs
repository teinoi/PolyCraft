using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.ProBuilder;

/// <summary>
/// ������ ���� �������� Raycast ��ġ�� ����
/// </summary>
public class MeshCreate : MonoBehaviour
{
    // ��Ʈ�ѷ��� Ʈ���� �Է� ����
    public InputActionReference triggerAction;

    // Ray�� �浹�� interactable�� ��ȣ�ۿ� ������ Interactor
    public XRRayInteractor rayInteractor;

    // XRInteractionManager ����
    public XRInteractionManager interactionManager;

    // ������ ������ ����Ʈ
    public List<GameObject> objectPrefabs;

    // ���� ���õ� ������
    GameObject currentPrefab;

    // ���� ���� ��忡�� �̹� �����ߴ��� ����
    bool hasCreatedObject = false;

    // ���� ���� ��� ����
    bool isSingleCreationMode = true;

    void OnEnable()
    {
        triggerAction.action.started += OnTriggerPressed;
        UIManager.OnShapeSelected += OnPrefabSelected;
    }

    void OnDisable()
    {
        triggerAction.action.started -= OnTriggerPressed;
        UIManager.OnShapeSelected -= OnPrefabSelected;
    }

    // Ʈ���� �Է� �� ������Ʈ ���� �õ�
    void OnTriggerPressed(InputAction.CallbackContext context)
    {
        // �������� ���õǾ� �ְ� Ray�� �浹���� ���
        if (currentPrefab != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // ���� ���� ��尡 �ƴϰų� ���� ���� �� �� ���
            if (!isSingleCreationMode || !hasCreatedObject)
            {
                CreateObjectAtPosition(hit.point);
                hasCreatedObject = true;
            }
        }
    }

    // ������Ʈ�� ������ ��ġ�� ���� �� �ʱ� ����
    void CreateObjectAtPosition(Vector3 position)
    {
        // Ray�� �浹�ߴ� ��ġ�� ������ �������� ����
        GameObject createdObject = Instantiate(currentPrefab);
        createdObject.transform.position = position;

        // Pivot�� �޽��� �߾����� ����
        CenterPivot(createdObject);

        // ������ ������Ʈ�� Collider, Rigidbody ������Ʈ ���� �߰�
        ComponentManager.AddComponents(createdObject, interactionManager);
    }

    // Pivot�� �޽��� �߾����� ����
    void CenterPivot(GameObject obj)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3 centroid = Vector3.zero;

            // ��� ������ ���� �߽� ���
            foreach (Vector3 vertex in vertices)
            {
                centroid += vertex;
            }

            centroid /= vertices.Length;
            Vector3 offset = centroid;

            // ������ �߽� �������� �̵�
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= offset;
            }

            // ����� ���� ����
            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            // ������Ʈ ��ġ�� ������ �߽ɿ� ���� �̵�
            obj.transform.position += offset;

            // MeshCollider�� �ִٸ� ������Ʈ
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = mesh;
            }
        }
    }

    // UI���� ����(������)�� �������� �� ȣ��
    void OnPrefabSelected(ShapeType shapeType)
    {
        currentPrefab = objectPrefabs[(int)shapeType];
        hasCreatedObject = false;
    }

    // ���� ��� ���
    public void ToggleCreateMode()
    {
        isSingleCreationMode = !isSingleCreationMode;
        hasCreatedObject = false;

        if (isSingleCreationMode)
        {
            hasCreatedObject = true;
        }
    }
}