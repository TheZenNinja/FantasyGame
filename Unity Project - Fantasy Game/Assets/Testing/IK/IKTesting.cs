using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IKTesting : MonoBehaviour
{
    public int chainLength = 2;
    public Transform target;

    public float[] bonesLength;
    public float completeLength;
    public Transform[] bones;
    public Vector3[] positions;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];

        completeLength = 0;

        Transform current = transform;

        for (int i = bones.Length - 1; i >= 0; i--)
        {
            current = current.parent;

            if (i == bones.Length - 1)
            {

            }
            else
            {
                bonesLength[i] = (bones[i + 1].position - current.position).magnitude;
                completeLength += bonesLength[i];
            }
        }
    }

    void LateUpdate()
    {
        ResolveIK();
    }
    public void ResolveIK()
    {
        if (target == null)
            return;

        if (bonesLength.Length != chainLength)
            Init();

        for (int i = 0; i < bones.Length; i++)
            positions[i] = bones[i].position;

        if ((target.position - bones[0].position).sqrMagnitude >= completeLength * completeLength)
        {
            Vector3 dir = (target.position - positions[0]).normalized;

            for (int i = 0; i < positions.Length; i++)

                positions[i] = positions[i - 1] + dir * bonesLength[i - 1];
        }

        for (int i = 0; i < positions.Length; i++)
            bones[i].position = positions[i];
    }
    private void OnDrawGizmos()
    {
        Transform current = transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;

            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
