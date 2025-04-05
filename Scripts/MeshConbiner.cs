using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Outline이 활성화되어 있는 메시들을 하나로 병합
/// </summary>
public class MeshCombiner : MonoBehaviour
{
    // XRInteractionManager 참조
    public XRInteractionManager interactionManager;

    // 선택된(Outline이 활성화된) 오브젝트들을 메시 병합
    public void CombineMeshesWithOutline()
    {
        // Outline 컴포넌트가 있는 오브젝트들을 찾기
        Outline[] allOutlines = FindObjectsOfType<Outline>();
        List<MeshFilter> meshFiltersToCombine = new List<MeshFilter>();

        // Outline이 활성화된 오브젝트만 병합 대상으로 필터링
        foreach (Outline outline in allOutlines)
        {
            if (outline.enabled)
            {
                MeshFilter meshFilter = outline.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFiltersToCombine.Add(meshFilter);
                }
            }
        }

        // 병합 대상이 없으면 종료
        if (meshFiltersToCombine.Count == 0)
        {
            return;
        }

        // 메시 병합 실행
        CombineMeshes(meshFiltersToCombine);
    }

    // 전달받은 MeshFilter 리스트를 병합하여 오브젝트 생성
    void CombineMeshes(List<MeshFilter> meshFilters)
    {
        // 모든 메시의 중심 위치를 평균 내어 병합 메시의 Pivot으로 사용
        Vector3 center = Vector3.zero;
        foreach (var meshFilter in meshFilters)
        {
            center += meshFilter.transform.position;
        }
        center /= meshFilters.Count;

        // 병합할 메시 배열 준비
        Mesh combinedMesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        for (int i = 0; i < meshFilters.Count; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            // 기존 오브젝트는 병합 후 비활성화
            meshFilters[i].gameObject.SetActive(false);
        }

        // 메시 병합 수행
        combinedMesh.CombineMeshes(combine);

        // 병합된 메시를 담을 오브젝트 생성
        GameObject combinedObject = new GameObject("CombinedMesh");

        // 오브젝트에 MeshFilter, MeshRenderer, MeshCollider 컴포넌트 추가
        MeshFilter meshFilterCombined = combinedObject.AddComponent<MeshFilter>();
        meshFilterCombined.mesh = combinedMesh;
        combinedObject.AddComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().material;
        combinedObject.AddComponent<MeshCollider>().sharedMesh = combinedMesh;

        // 병합된 오브젝트를 계산된 중심 위치에 배치
        combinedObject.transform.position = center;

        // 메시의 정점들을 기준점에서부터의 위치로 보정 (중심 정렬)
        Vector3[] vertices = combinedMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center;
        }
        combinedMesh.vertices = vertices;
        combinedMesh.RecalculateBounds();

        // 생성된 오브젝트에 Collider, Rigidbody 컴포넌트 등을 추가
        ComponentManager.AddComponents(combinedObject, interactionManager);

        // XRGrabInteractable 초기화 버그로 인해 회피용
        combinedObject.SetActive(false);
        combinedObject.SetActive(true);
    }
}