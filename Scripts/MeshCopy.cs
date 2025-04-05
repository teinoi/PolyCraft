using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Outline이 활성화되어 있는 오브젝트를 복제
/// </summary>
public class MeshCopy : MonoBehaviour
{
    // XRInteractionManager 참조
    public XRInteractionManager interactionManager;

    // 복제된 오브젝트의 위치 오프셋
    public Vector3 copyPosition;

    // Outline이 활성화된 모든 오브젝트를 찾아 복제하고 새로운 위치에 생성
    public void DuplicateMeshesWithOutline()
    {
        // Outline 컴포넌트가 있는 오브젝트들을 찾기
        Outline[] allOutlines = FindObjectsOfType<Outline>();

        foreach (Outline outline in allOutlines)
        {
            if (outline.enabled)
            {
                // 원본 오브젝트 참조
                GameObject originalObject = outline.gameObject;

                // 오브젝트 복제
                GameObject duplicateObject = Instantiate(originalObject);

                // 복제 오브젝트 위치 조정(기존 위치 + copyPosition)
                Vector3 newPosition = originalObject.transform.position + copyPosition;
                duplicateObject.transform.position = newPosition;

                // 복제된 오브젝트의 Outline 비활성화
                Outline duplicateOutline = duplicateObject.GetComponent<Outline>();
                if (duplicateOutline != null)
                {
                    duplicateOutline.enabled = false;
                }

                // 불필요한 머티리얼 제거
                RemoveUnwantedMaterials(duplicateObject);

                // 생성된 오브젝트에 Collider, Rigidbody 컴포넌트 등을 추가
                ComponentManager.AddComponents(duplicateObject, interactionManager);
            }
        }
    }

    // 복제된 오브젝트의 렌더러에서 Outline 관련 머티리얼만 필터링하여 제거
    void RemoveUnwantedMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] originalMaterials = renderer.sharedMaterials;
            List<Material> newMaterials = new List<Material>();

            foreach (Material material in originalMaterials)
            {
                if (!(material.name.Contains("OutlineMask") || material.name.Contains("OutlineFill")))
                {
                    newMaterials.Add(material);
                }
            }

            renderer.materials = newMaterials.ToArray();
        }
    }
}