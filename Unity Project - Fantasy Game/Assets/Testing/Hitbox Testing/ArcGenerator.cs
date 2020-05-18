using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcGenerator : MonoBehaviour
{
    public Vector3[] verts;
    int[] tris;

    Mesh mesh;
    MeshFilter filter;



    public int resolution = 10;
    public float radius = 10;
    [Range(-360, 360)]
    public float startAngle;
    [Range(0, 21)]
    public int index;
    float baseAngle = 90;
    void GenPoints()
    {
        //verts = new Vector3[resolution*2 + 2];
        verts = new Vector3[resolution + 1];

        verts[0] = Vector3.zero;
        verts[1] = Vector3.forward * radius;
        verts[resolution - 1] = Vector3.right * radius;
        if (resolution % 2 == 0)
        {
            for (int i = 0; i < resolution-1; i++)
            {
                float angle = baseAngle / resolution * i;
                verts[i + 2] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
            }
        }
        //verts[1] = Vector3.up;

        /*for (var i = 0; i < resolution; i++)//lower layer
        {
            float angle = (startAngle + 90f) / resolution * i;
            verts[i+2] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
        }
        for (var i = 0; i < resolution; i++)//upper layer
        {
            float angle = (startAngle + 90f) / resolution * i;
            verts[i+resolution+2] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 1/radius, Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
        }*/
    }
    public void GenTris()
    {
        tris = new int[ (2 + (resolution-1) * 3)* 6];

        tris[0] = 0;
        tris[1] = 2;
        tris[2] = resolution + 2;

        tris[3] = resolution + 2;
        tris[4] = 1;
        tris[5] = 0;

        int startPos = 6;
        int vert = 0;
        for (int i = 0; i < resolution - 1; i++)
        {
            tris[startPos + 0] = i + 2;
            tris[startPos + 1] = i + 3;
            tris[startPos + 2] = i + resolution + 3;

            tris[startPos + 3] = i + resolution + 3;
            tris[startPos + 4] = i + resolution + 2;
            tris[startPos + 5] = i + 2;

            vert++;
            startPos += 6;
        }

        /*tris[3] = 1;
        tris[4] = resolution + 1;
        tris[5] = resolution + 2;*/

        /*int vert = 0;
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
        }*/
    }

    

    void AssignMesh()
    {
        if (mesh == null)
            mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;

        filter.mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    private void OnValidate()
    {
        GenPoints();
        //GenTris();
        //AssignMesh();
    }
    private void OnDrawGizmos()
    {

        if (verts.Length > 0)
            for (int i = 0; i < verts.Length; i++)
            {
                if (i == index)
                    Gizmos.color = Color.green;
                else if (i % 2 == 0)
                    Gizmos.color = Color.black;
                else
                    Gizmos.color = Color.white;

                Gizmos.DrawSphere(transform.position + verts[i], 0.25f);
            }
    }
}
