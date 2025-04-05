using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// ������ ������Ʈ�� �ʼ� ������Ʈ�� �߰�
/// </summary>
public static class ComponentManager
{
    // ������Ʈ�� ������Ʈ�� �߰��ϰ� �ʱ� ����
    public static void AddComponents(GameObject obj, XRInteractionManager interactionManager)
    {
        // MeshCollider ������Ʈ �߰�
        if (obj.GetComponent<MeshCollider>() == null)
        {
            MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }

        // Rigidbody ������Ʈ �߰�
        if (obj.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        // RotationLimit ������Ʈ �߰�
        RotationLimit rotateLimit = obj.GetComponent<RotationLimit>();

        if (rotateLimit == null)
        {
            rotateLimit = obj.AddComponent<RotationLimit>();
            rotateLimit.interactionManager = interactionManager;
        }

        // Outline ������Ʈ �߰�
        Outline outline = obj.GetComponent<Outline>();

        if (outline == null)
        {
            outline = obj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color(0, 160, 255);
            outline.OutlineWidth = 2f;
            outline.enabled = false;
        }

        // ������Ʈ�� ���̾� ����
        obj.layer = 6;
    }
}