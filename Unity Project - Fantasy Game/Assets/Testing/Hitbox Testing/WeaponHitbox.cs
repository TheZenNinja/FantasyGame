using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class WeaponHitbox : MonoBehaviour
{
    [System.Serializable]
    public class HitboxShape
    {
        public enum HitboxType
        {
            arc,
            sphere,
            box,
        }
        public HitboxType shape = HitboxType.arc;
        public Vector3 center;

        [Header("Arc")]
        public float innerRad;
        public float outerRad;
        [Range(-360, 360)]
        public float leftAngle, rightAngle;

        public float thickness = 1;
        public int curveResolution = 10;


        public Mesh GenerateArc()
        {
            Vector3[] verts = new Vector3[(curveResolution + 1) * 2];

            verts[0] = -Vector3.up * thickness;
            verts[1] = Vector3.up *thickness;

            for (var i = 0; i < curveResolution; i++)//lower layer
            {
                float angle = (leftAngle + rightAngle) / curveResolution * i;
                verts[i + 2] = new Vector3(Mathf.Sin((angle-leftAngle) * Mathf.Deg2Rad), thickness / 2 / outerRad, Mathf.Cos((angle - leftAngle) * Mathf.Deg2Rad)) * outerRad;
            }
            for (var i = 0; i < curveResolution; i++)//upper layer
            {
                float angle = (leftAngle + rightAngle) / curveResolution * i;
                verts[i + 2 + curveResolution] = new Vector3(Mathf.Sin((angle - leftAngle) * Mathf.Deg2Rad), -thickness / 2 / outerRad, Mathf.Cos((angle - leftAngle) * Mathf.Deg2Rad)) * outerRad;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = verts;
            mesh.triangles = new int[] { 0,1,2};
            return mesh;
        }
    }
    public HitboxShape hitbox;
    public Transform player;

    public GameObject meshTest;

    public MeshCollider collider;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        if (hitbox.shape == HitboxShape.HitboxType.arc)
        {
            Vector3 angleLeft = DirFromAngle(-hitbox.leftAngle);
            Vector3 angleRight = DirFromAngle(hitbox.rightAngle);

            Handles.DrawLine(player.position + angleLeft * hitbox.innerRad, player.position + angleLeft * hitbox.outerRad);
            Handles.DrawLine(player.position + angleRight * hitbox.innerRad, player.position + angleRight * hitbox.outerRad);

            float angle = hitbox.leftAngle + hitbox.rightAngle;

            Vector3 fwd = angleLeft;// DirFromAngle(angle / 2- angle);

            Handles.DrawWireArc(player.position, Vector3.up, fwd, angle, hitbox.innerRad);
            Handles.DrawWireArc(player.position, Vector3.up, fwd, angle, hitbox.outerRad);
        }
    }
    [ContextMenu("Generate Shape")]
    public void GenerateMesh()
    {
        collider.sharedMesh = hitbox.GenerateArc();
    }

    public Vector3 DirFromAngle(float angleDeg)
    {
        angleDeg += player.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }
}
