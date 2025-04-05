using UnityEngine;

/// <summary>
/// Outline이 활성화되어 있는 오브젝트를 삭제
/// </summary>
public class MeshDelete : MonoBehaviour
{
    public void DeleteMeshesWithOutline()
    {
        // Outline 컴포넌트가 있는 오브젝트들을 찾기
        Outline[] allOutlines = FindObjectsOfType<Outline>();

        foreach (Outline outline in allOutlines)
        {
            // Outline이 활성화된 오브젝트를 파괴
            if (outline.enabled)
            {
                Destroy(outline.gameObject);
            }
        }
    }
}