using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WeaponTrail : MonoBehaviour
{
    public Transform top,bottom;

    public float speed = 10;

    Vector3[] pointsA;
    Vector3[] localPointsA = new Vector3[0];
    int[] trisA;
    Mesh meshA;
    MeshFilter filterA;
    MeshRenderer rendererA;

   public Material prefabMaterial;

    void Update()
    {
        UpdatePoints();
    }
    public void UpdatePoints()
    {
        if (pointsA.Length != 4)
        {
            pointsA = new Vector3[4];

            pointsA[0] = bottom.position;
            pointsA[1] = top.position;

            pointsA[2] = bottom.position;
            pointsA[3] = top.position;
        }
        else
        {
            pointsA[0] = bottom.position;
            pointsA[1] = top.position;

            pointsA[2] = Vector3.Lerp(pointsA[2], bottom.position, Time.deltaTime * speed);
            pointsA[3] = Vector3.Lerp(pointsA[3], top.position, Time.deltaTime * speed);
        }


        if (localPointsA.Length != pointsA.Length)
            localPointsA = new Vector3[pointsA.Length];

        for (int i = 0; i < pointsA.Length; i++)
            localPointsA[i] = transform.InverseTransformPoint(pointsA[i]);


        meshA.vertices = localPointsA;
        GenTris();
        meshA.triangles = trisA;
        meshA.RecalculateNormals();

    }

    public void GenTris()
    {
        //trisA = new int[12];
        trisA = new int[6];
        trisA[0] = 0;
        trisA[1] = 1;
        trisA[2] = 2;

        trisA[3] = 1;
        trisA[4] = 3;
        trisA[5] = 2;

        /*trisA[6] = 2;
        trisA[7] = 1;
        trisA[8] = 0;

        trisA[9] = 2;
        trisA[10] = 3;
        trisA[11] = 1;*/

        /*tris[triCount + 0] = vert + 0;
        tris[triCount + 1] = vert + points.Length/2 + 1;
        tris[triCount + 2] = vert + 1;
        tris[triCount + 3] = vert + 1;
        tris[triCount + 4] = vert + points.Length / 2 + 1;
        tris[triCount + 5] = vert + points.Length / 2 + 2;*/


        /*tris = new int[Mathf.RoundToInt( (float)points.Count / 4 * 6 )];

        int vert = 0;
        int triCount = 0;
        
        for (int i = 1; i < points.Count / 2; i++)
        {
            tris[triCount + 0] = vert + 0;
            tris[triCount + 1] = vert + 1;
            tris[triCount + 2] = vert + 2;


            tris[triCount + 3] = vert + 1;
            tris[triCount + 4] = vert + 3;
            tris[triCount + 5] = vert + 2;

            vert += 4;
            triCount += 6;
        }*/
    }

    [ContextMenu(nameof(Initialize))]
    void Initialize()
    {
        pointsA = new Vector3[4];

        if (meshA == null)
        {
            meshA = new Mesh();
            meshA.name = "trail mesh";
        }
        if (filterA == null)
        {
            filterA = gameObject.AddComponent<MeshFilter>();
            filterA.mesh = meshA;
        }
        if (rendererA == null)
            rendererA = gameObject.AddComponent<MeshRenderer>();
        Material mat = Instantiate(prefabMaterial);
        mat.SetFloat("_Cull", 0);
        rendererA.material = mat;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < localPointsA.Length; i++)
            Gizmos.DrawWireSphere(transform.position + localPointsA[i], 0.1f);

    }
}
