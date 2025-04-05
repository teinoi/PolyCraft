using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Outline이 활성화되어 있는 오브젝트의 머티리얼 색상을 변경
/// </summary>
public class MeshMaterial : MonoBehaviour
{
    // OutlineManager 참조
    public OutlineManager outlineManager;

    Color selectedColor;

    // 머티리얼 적용 시작
    public void StartApplying()
    {
        ApplyMaterial();
    }

    // 선택된 색상으로 머티리얼 적용
    void ApplyMaterial()
    {
        // 아웃라인 된 객체 리스트 가져오기
        var outlinedObjects = outlineManager?.GetOutlinedObjects();

        if (outlinedObjects != null && outlinedObjects.Count > 0)
        {
            foreach (Transform obj in outlinedObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // 현재 머티리얼의 색상만 변경
                    renderer.material.color = selectedColor;
                }
            }
        }
    }

    // 색상 선택
    public void SetColor(Color color)
    {
        selectedColor = color;
    }
}