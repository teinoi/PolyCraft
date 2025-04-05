using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Outline�� Ȱ��ȭ�Ǿ� �ִ� �޽õ��� �ϳ��� ����
/// </summary>
public class MeshCombiner : MonoBehaviour
{
    // XRInteractionManager ����
    public XRInteractionManager interactionManager;

    // ���õ�(Outline�� Ȱ��ȭ��) ������Ʈ���� �޽� ����
    public void CombineMeshesWithOutline()
    {
        // Outline ������Ʈ�� �ִ� ������Ʈ���� ã��
        Outline[] allOutlines = FindObjectsOfType<Outline>();
        List<MeshFilter> meshFiltersToCombine = new List<MeshFilter>();

        // Outline�� Ȱ��ȭ�� ������Ʈ�� ���� ������� ���͸�
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

        // ���� ����� ������ ����
        if (meshFiltersToCombine.Count == 0)
        {
            return;
        }

        // �޽� ���� ����
        CombineMeshes(meshFiltersToCombine);
    }

    // ���޹��� MeshFilter ����Ʈ�� �����Ͽ� ������Ʈ ����
    void CombineMeshes(List<MeshFilter> meshFilters)
    {
        // ��� �޽��� �߽� ��ġ�� ��� ���� ���� �޽��� Pivot���� ���
        Vector3 center = Vector3.zero;
        foreach (var meshFilter in meshFilters)
        {
            center += meshFilter.transform.position;
        }
        center /= meshFilters.Count;

        // ������ �޽� �迭 �غ�
        Mesh combinedMesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        for (int i = 0; i < meshFilters.Count; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            // ���� ������Ʈ�� ���� �� ��Ȱ��ȭ
            meshFilters[i].gameObject.SetActive(false);
        }

        // �޽� ���� ����
        combinedMesh.CombineMeshes(combine);

        // ���յ� �޽ø� ���� ������Ʈ ����
        GameObject combinedObject = new GameObject("CombinedMesh");

        // ������Ʈ�� MeshFilter, MeshRenderer, MeshCollider ������Ʈ �߰�
        MeshFilter meshFilterCombined = combinedObject.AddComponent<MeshFilter>();
        meshFilterCombined.mesh = combinedMesh;
        combinedObject.AddComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().material;
        combinedObject.AddComponent<MeshCollider>().sharedMesh = combinedMesh;

        // ���յ� ������Ʈ�� ���� �߽� ��ġ�� ��ġ
        combinedObject.transform.position = center;

        // �޽��� �������� ���������������� ��ġ�� ���� (�߽� ����)
        Vector3[] vertices = combinedMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center;
        }
        combinedMesh.vertices = vertices;
        combinedMesh.RecalculateBounds();

        // ������ ������Ʈ�� Collider, Rigidbody ������Ʈ ���� �߰�
        ComponentManager.AddComponents(combinedObject, interactionManager);

        // XRGrabInteractable �ʱ�ȭ ���׷� ���� ȸ�ǿ�
        combinedObject.SetActive(false);
        combinedObject.SetActive(true);
    }
}