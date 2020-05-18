using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class MeshConverter : MonoBehaviour
{
    Mesh mesh;
    [Range(0.01f, 0.1f)]
    public float size = 0.01f;

    public Vector3[] verts;
    public Vector3[] normals;

    private void OnValidate()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }
    private void Update()
    {
        if (mesh)
        {
            verts = mesh.vertices;
            normals = mesh.normals;
        }
    }
    private void OnDrawGizmos()
    {
        if (mesh)
        {
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Vector3 pos = transform.position + mesh.vertices[i];

                Gizmos.DrawSphere(pos, size);
                Gizmos.DrawLine(pos, pos + (mesh.normals[i]/10));
            }
            
        }
    }
}
