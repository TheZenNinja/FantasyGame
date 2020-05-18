using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one * 5;

    Vector3[] verts;
    int[] tris;

    Mesh mesh;
    MeshFilter filter;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnValidate()
    {
        if (filter == null)
            filter = GetComponent<MeshFilter>();
        if (mesh == null)
            mesh = new Mesh();

        CreateShape();
        GenTris();
        AssignMesh();
    }

    void CreateShape()
    {
        verts = new Vector3[((size.x + 1) * (size.y + 1))];


        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x <= size.x; x++, i++)
            {
                verts[i] = new Vector3(x, 0, y);
            }
        }
    }

    public void GenTris()
    {
        tris = new int[size.x * size.y * 6];

        int vert = 0;
        int triCount = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                tris[triCount + 0] = vert + 0;
                tris[triCount + 1] = vert + size.x + 1;
                tris[triCount + 2] = vert + 1;
                tris[triCount + 3] = vert + 1;
                tris[triCount + 4] = vert + size.x + 1;
                tris[triCount + 5] = vert + size.x + 2;

                vert++;
                triCount += 6;
            }
            vert++;
        }
    }

    void AssignMesh()
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;

        filter.mesh = mesh;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (verts.Length > 0)
            for (int i = 0; i < verts.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + verts[i], 0.1f);
            }
    }
}
