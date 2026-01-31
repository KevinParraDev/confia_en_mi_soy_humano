using System;
using UnityEngine;

public class NPCFOVController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    private Mesh mesh;
    Vector3 origin;
    private float startingAngle;

    float fov = 90f;

    // This is expensive be careful
    [SerializeField] int rayCount = 2;
    float viewDistance = 3f;

    public void Initialize(float _fov = 360f, float _viewDistance = 0.5f)
    {
        fov = _fov;
        viewDistance = _viewDistance;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        float angle = startingAngle;
        ;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D rayCastHit2d = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            // Debug.DrawRay(origin, GetVectorFromAngle(angle) * viewDistance, Color.red, 1f);
            if (rayCastHit2d.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = rayCastHit2d.point;

            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                // First Triangle
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            // Because unity go counter clockwise we decrease
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 _origin)
    {
        this.origin = _origin;
    }

    public void SetAimDirection(Vector3 _aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(_aimDirection) + fov / 2f;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    } 

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }
}
