using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.ProBuilder;

/// <summary>
/// 선택한 도형 프리팹을 Raycast 위치에 생성
/// </summary>
public class MeshCreate : MonoBehaviour
{
    // 컨트롤러의 트리거 입력 참조
    public InputActionReference triggerAction;

    // Ray에 충돌된 interactable과 상호작용 가능한 Interactor
    public XRRayInteractor rayInteractor;

    // XRInteractionManager 참조
    public XRInteractionManager interactionManager;

    // 생성할 프리팹 리스트
    public List<GameObject> objectPrefabs;

    // 현재 선택된 프리팹
    GameObject currentPrefab;

    // 단일 생성 모드에서 이미 생성했는지 여부
    bool hasCreatedObject = false;

    // 단일 생성 모드 여부
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

    // 트리거 입력 시 오브젝트 생성 시도
    void OnTriggerPressed(InputAction.CallbackContext context)
    {
        // 프리팹이 선택되어 있고 Ray가 충돌했을 경우
        if (currentPrefab != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // 단일 생성 모드가 아니거나 아직 생성 안 한 경우
            if (!isSingleCreationMode || !hasCreatedObject)
            {
                CreateObjectAtPosition(hit.point);
                hasCreatedObject = true;
            }
        }
    }

    // 오브젝트를 지정한 위치에 생성 및 초기 설정
    void CreateObjectAtPosition(Vector3 position)
    {
        // Ray가 충돌했던 위치에 선택한 프리팹을 생성
        GameObject createdObject = Instantiate(currentPrefab);
        createdObject.transform.position = position;

        // Pivot을 메시의 중앙으로 조정
        CenterPivot(createdObject);

        // 생성된 오브젝트에 Collider, Rigidbody 컴포넌트 등을 추가
        ComponentManager.AddComponents(createdObject, interactionManager);
    }

    // Pivot을 메시의 중앙으로 조정
    void CenterPivot(GameObject obj)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3 centroid = Vector3.zero;

            // 모든 정점의 무게 중심 계산
            foreach (Vector3 vertex in vertices)
            {
                centroid += vertex;
            }

            centroid /= vertices.Length;
            Vector3 offset = centroid;

            // 정점을 중심 기준으로 이동
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= offset;
            }

            // 변경된 정점 적용
            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            // 오브젝트 위치를 보정된 중심에 맞춰 이동
            obj.transform.position += offset;

            // MeshCollider가 있다면 업데이트
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = mesh;
            }
        }
    }

    // UI에서 도형(프리팹)을 선택했을 때 호출
    void OnPrefabSelected(ShapeType shapeType)
    {
        currentPrefab = objectPrefabs[(int)shapeType];
        hasCreatedObject = false;
    }

    // 생성 모드 토글
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