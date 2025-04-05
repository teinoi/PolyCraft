using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� ����
/// </summary>
public class MeshCopy : MonoBehaviour
{
    // XRInteractionManager ����
    public XRInteractionManager interactionManager;

    // ������ ������Ʈ�� ��ġ ������
    public Vector3 copyPosition;

    // Outline�� Ȱ��ȭ�� ��� ������Ʈ�� ã�� �����ϰ� ���ο� ��ġ�� ����
    public void DuplicateMeshesWithOutline()
    {
        // Outline ������Ʈ�� �ִ� ������Ʈ���� ã��
        Outline[] allOutlines = FindObjectsOfType<Outline>();

        foreach (Outline outline in allOutlines)
        {
            if (outline.enabled)
            {
                // ���� ������Ʈ ����
                GameObject originalObject = outline.gameObject;

                // ������Ʈ ����
                GameObject duplicateObject = Instantiate(originalObject);

                // ���� ������Ʈ ��ġ ����(���� ��ġ + copyPosition)
                Vector3 newPosition = originalObject.transform.position + copyPosition;
                duplicateObject.transform.position = newPosition;

                // ������ ������Ʈ�� Outline ��Ȱ��ȭ
                Outline duplicateOutline = duplicateObject.GetComponent<Outline>();
                if (duplicateOutline != null)
                {
                    duplicateOutline.enabled = false;
                }

                // ���ʿ��� ��Ƽ���� ����
                RemoveUnwantedMaterials(duplicateObject);

                // ������ ������Ʈ�� Collider, Rigidbody ������Ʈ ���� �߰�
                ComponentManager.AddComponents(duplicateObject, interactionManager);
            }
        }
    }

    // ������ ������Ʈ�� ���������� Outline ���� ��Ƽ���� ���͸��Ͽ� ����
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