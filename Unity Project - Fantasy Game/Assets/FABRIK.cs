using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[ExecuteAlways]
public class FABRIK : MonoBehaviour
{
    public Transform[] bones;
    public Transform target;
    public Transform pole;
    public Vector3[] points;
    public float[] lengths;

    public float totalLength;

    public float smoothSpeed = 5;

    [Range(0, 1)]
    public float poleStrength;

    public int iterations = 5;
    private int poleIter;
    private int halfStop;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        points = new Vector3[bones.Length];
        lengths = new float[bones.Length - 1];

        totalLength = 0;
        for (int i = 0; i < lengths.Length; i++)
        {
            lengths[i] = Vector3.Magnitude(bones[i].position - bones[i + 1].position);
            totalLength += lengths[i];
        }


        halfStop = bones.Length % 2 == 0 ? bones.Length / 2 : Mathf.CeilToInt((float)bones.Length / 2);

    }

    void Update()
    {
        if (target == null)
            return;
        if (points.Length != bones.Length)
            Init();

        ResolveIK();
    }
    void ResolveIK()
    {
        for (int i = 0; i < points.Length; i++)
            points[i] = bones[i].position;

        

        if ((target.position - points[0]).sqrMagnitude >= totalLength * totalLength)
        {
            ResolveRotation(true);
            Vector3 dir = (target.position - points[0]).normalized;

            for (int i = 0; i < points.Length; i++)
            {
                if (i > 0)
                    points[i] = points[i - 1] + lengths[i - 1] * dir;
            }
        }
        else
        {
            //ResolveRotation(false);

            for (int i = 0; i < poleIter; i++)
            {
                BackwardIK(true);
                ForwardIK(true);
            }
            for (int i = 0; i < iterations - poleIter; i++)
            {
                BackwardIK(false);
                ForwardIK(false);
            }
        }

        for (int i = 0; i < points.Length; i++)
            bones[i].position = Vector3.Lerp(bones[i].position, points[i], Time.unscaledDeltaTime * smoothSpeed);
    }

    void ForwardIK(bool usePole)
    {
        if (pole == null)
            usePole = false;

        points[0] = bones[0].position;
        for (int i = 1; i < points.Length - 1; i++)
        {
            Vector3 dir;
            if (usePole)
                dir = (pole.position - points[i - 1]).normalized;
            else
                dir = (points[i] - points[i - 1]).normalized;
            points[i] = points[i - 1] + lengths[i - 1] * dir;
        }
    }
    void BackwardIK(bool usePole)
    {
        if (pole == null)
            usePole = false;

        points[points.Length - 1] = target.position;
        for (int i = points.Length - 2; i > 0; i--)
        {
            Vector3 dir;
            if (usePole)
                dir = (pole.position - points[i - 1]).normalized;
            else
            dir = (points[i] - points[i + 1]).normalized;
            points[i] = points[i + 1] + lengths[i] * dir;
        }
    }

    void ResolveRotation(bool overTotalLength)
    {
        if (overTotalLength)
        {
            for (int i = 0; i < bones.Length - 1; i++)
                bones[i].up = (target.position - bones[i].position).normalized;
        }
        else
        {
            for (int i = 0; i < bones.Length - 1; i++)
            {
                bones[i].up = (bones[i + 1].position - bones[i].position).normalized;
            }
            bones[bones.Length-1].up = target.up ;
        }
    }

    private void OnValidate()
    {
        poleIter = Mathf.RoundToInt(iterations * poleStrength);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < bones.Length; i++)
        {
            Gizmos.DrawLine(bones[i].position, bones[i].position + bones[i].up);
        }

        /*Gizmos.color = Color.red;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], 0.5f);
        }*/
    }
}
