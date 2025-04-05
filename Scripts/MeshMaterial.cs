using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Outline�� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ�� ��Ƽ���� ������ ����
/// </summary>
public class MeshMaterial : MonoBehaviour
{
    // OutlineManager ����
    public OutlineManager outlineManager;

    Color selectedColor;

    // ��Ƽ���� ���� ����
    public void StartApplying()
    {
        ApplyMaterial();
    }

    // ���õ� �������� ��Ƽ���� ����
    void ApplyMaterial()
    {
        // �ƿ����� �� ��ü ����Ʈ ��������
        var outlinedObjects = outlineManager?.GetOutlinedObjects();

        if (outlinedObjects != null && outlinedObjects.Count > 0)
        {
            foreach (Transform obj in outlinedObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // ���� ��Ƽ������ ���� ����
                    renderer.material.color = selectedColor;
                }
            }
        }
    }

    // ���� ����
    public void SetColor(Color color)
    {
        selectedColor = color;
    }
}