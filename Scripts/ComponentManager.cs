using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// 생성된 오브젝트에 필수 컴포넌트를 추가
/// </summary>
public static class ComponentManager
{
    // 오브젝트에 컴포넌트를 추가하고 초기 설정
    public static void AddComponents(GameObject obj, XRInteractionManager interactionManager)
    {
        // MeshCollider 컴포넌트 추가
        if (obj.GetComponent<MeshCollider>() == null)
        {
            MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }

        // Rigidbody 컴포넌트 추가
        if (obj.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        // RotationLimit 컴포넌트 추가
        RotationLimit rotateLimit = obj.GetComponent<RotationLimit>();

        if (rotateLimit == null)
        {
            rotateLimit = obj.AddComponent<RotationLimit>();
            rotateLimit.interactionManager = interactionManager;
        }

        // Outline 컴포넌트 추가
        Outline outline = obj.GetComponent<Outline>();

        if (outline == null)
        {
            outline = obj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color(0, 160, 255);
            outline.OutlineWidth = 2f;
            outline.enabled = false;
        }

        // 오브젝트의 레이어 설정
        obj.layer = 6;
    }
}