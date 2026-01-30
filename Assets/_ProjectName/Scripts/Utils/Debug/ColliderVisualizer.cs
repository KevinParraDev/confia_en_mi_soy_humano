using UnityEngine;
using System.Collections.Generic;

public class ColliderVisualizer : MonoBehaviour
{
    public Color colliderColor = Color.green;
    public bool drawWireframe = true;

    private Material lineMaterial;
    private List<Collider> colliders = new List<Collider>();

    private void Awake()
    {
        lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;

        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        lineMaterial.SetInt("_ZWrite", 0);

        colliders.AddRange(GetComponents<Collider>());
    }

    private void OnRenderObject()
    {
        if (!Globals.DebugCollisionActivated)
            return;

        if (lineMaterial == null)
            return;
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(drawWireframe ? GL.LINES : GL.TRIANGLES);
        GL.Color(colliderColor);

        foreach (Collider collider in colliders)
        {
            if (collider.enabled)
            {
                if (collider is BoxCollider boxCollider)
                    DrawBoxCollider(boxCollider);
                else if (collider is CapsuleCollider capsuleCollider)
                    DrawCapsuleCollider(capsuleCollider);
                else if (collider is CharacterController characterController)
                    DrawCharacterController(characterController);
            }
        }

        GL.End();
        GL.PopMatrix();
    }

    private void DrawBoxCollider(BoxCollider box)
    {
        Vector3 center = box.center;
        Vector3 size = box.size;

        Vector3[] vertices = new Vector3[8];

        for (int i = 0; i < 8; i++)
        {
            vertices[i] = center + new Vector3(
                ((i & 1) == 0 ? -1 : 1) * size.x / 2,
                ((i & 2) == 0 ? -1 : 1) * size.y / 2,
                ((i & 4) == 0 ? -1 : 1) * size.z / 2
            );
        }

        int[] indices = new int[]
        {
            0,1, 1,3, 3,2, 2,0,
            4,5, 5,7, 7,6, 6,4,
            0,4, 1,5, 2,6, 3,7
        };

        for (int i = 0; i < indices.Length; i += 2)
        {
            GL.Vertex(vertices[indices[i]]);
            GL.Vertex(vertices[indices[i + 1]]);
        }
    }

    private void DrawCapsuleCollider(CapsuleCollider capsule)
    {
        int segments = 16;
        float radius = capsule.radius;
        float height = Mathf.Max(0, capsule.height / 2 - radius);

        Vector3 up = Vector3.up * height;
        Vector3 down = Vector3.down * height;

        Quaternion rotation = Quaternion.identity;

        switch (capsule.direction)
        {
            case 0: // X-axis
                rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 1: // Y-axis (default)
                rotation = Quaternion.identity;
                break;
            case 2: // Z-axis
                rotation = Quaternion.Euler(90, 0, 0);
                break;
        }

        Matrix4x4 matrix = Matrix4x4.TRS(capsule.center, rotation, Vector3.one);

        DrawCapsule(segments, radius, up, down, matrix);
    }

    private void DrawCharacterController(CharacterController controller)
    {
        int segments = 16;
        float radius = controller.radius;
        float height = Mathf.Max(0, controller.height / 2 - radius);

        Vector3 up = Vector3.up * height;
        Vector3 down = Vector3.down * height;

        Matrix4x4 matrix = Matrix4x4.TRS(controller.center, Quaternion.identity, Vector3.one);

        DrawCapsule(segments, radius, up, down, matrix);
    }

    private void DrawCapsule(int segments, float radius, Vector3 upOffset, Vector3 downOffset, Matrix4x4 matrix)
    {
        for (int i = 0; i < segments; i++)
        {
            float theta1 = (float)i / segments * Mathf.PI * 2;
            float theta2 = (float)(i + 1) / segments * Mathf.PI * 2;

            Vector3 p1 = new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1)) * radius;
            Vector3 p2 = new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2)) * radius;

            // Círculo superior
            GL.Vertex(matrix.MultiplyPoint3x4(p1 + upOffset));
            GL.Vertex(matrix.MultiplyPoint3x4(p2 + upOffset));

            // Círculo inferior
            GL.Vertex(matrix.MultiplyPoint3x4(p1 + downOffset));
            GL.Vertex(matrix.MultiplyPoint3x4(p2 + downOffset));

            // Conexiones verticales
            GL.Vertex(matrix.MultiplyPoint3x4(p1 + upOffset));
            GL.Vertex(matrix.MultiplyPoint3x4(p1 + downOffset));
        }

        // Dibujar líneas entre los polos
        int halfSegments = segments / 2;
        for (int i = 0; i <= halfSegments; i++)
        {
            float theta = (float)i / halfSegments * Mathf.PI;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            Vector3 pTop = new Vector3(0, sinTheta * radius, cosTheta * radius) + upOffset;
            Vector3 pBottom = new Vector3(0, -sinTheta * radius, cosTheta * radius) + downOffset;

            GL.Vertex(matrix.MultiplyPoint3x4(pTop));
            GL.Vertex(matrix.MultiplyPoint3x4(pBottom));
        }
    }
}
