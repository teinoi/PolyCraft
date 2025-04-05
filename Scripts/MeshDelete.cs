using UnityEngine;

/// <summary>
/// Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� ����
/// </summary>
public class MeshDelete : MonoBehaviour
{
    public void DeleteMeshesWithOutline()
    {
        // Outline ������Ʈ�� �ִ� ������Ʈ���� ã��
        Outline[] allOutlines = FindObjectsOfType<Outline>();

        foreach (Outline outline in allOutlines)
        {
            // Outline�� Ȱ��ȭ�� ������Ʈ�� �ı�
            if (outline.enabled)
            {
                Destroy(outline.gameObject);
            }
        }
    }
}