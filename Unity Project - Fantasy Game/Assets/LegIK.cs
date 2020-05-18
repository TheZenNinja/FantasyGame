using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LegIK : MonoBehaviour
{
    [System.Serializable]
    private class Leg
    {
        public Transform[] bones;
        public Vector3[] rotOffests;
        public Transform target;
        public Transform pole;
        private Vector3[] points;
        private float[] lengths;

        private float totalLength;

        public static int iterations = 5;
        public static float smoothSpeed = 5;

        public void Init()
        {
            points = new Vector3[bones.Length];
            lengths = new float[bones.Length - 1];
            rotOffests = new Vector3[bones.Length];
            totalLength = 0;
            for (int i = 0; i < lengths.Length; i++)
            {
                lengths[i] = Vector3.Magnitude(bones[i].position - bones[i + 1].position);
                totalLength += lengths[i];
            }

        }

        public void ForwardIK(bool usePole)
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
        public void BackwardIK(bool usePole)
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

        public void ResolveIK()
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = bones[i].position;

            if ((target.position - points[0]).sqrMagnitude >= totalLength * totalLength)
            {
                Vector3 dir = (target.position - points[0]).normalized;

                for (int i = 0; i < points.Length; i++)
                {
                    if (i > 0)
                        points[i] = points[i - 1] + lengths[i - 1] * dir;
                }
            }
            else
            {
                BackwardIK(true);
                ForwardIK(true);
                for (int i = 1; i < iterations; i++)
                {
                    BackwardIK(false);
                    ForwardIK(false);
                }
            }

            ResolveRotation();

            for (int i = 0; i < points.Length; i++)
                bones[i].position = Vector3.Lerp(bones[i].position, points[i], Time.unscaledDeltaTime * smoothSpeed);
        }

        void ResolveRotation()
        {
            if ((target.position - points[0]).sqrMagnitude >= totalLength * totalLength)
            {
                for (int i = 0; i < bones.Length - 1; i++)
                {
                    bones[i].up = (target.position - bones[i].position).normalized;
                    bones[i].localEulerAngles += rotOffests[i];
                }

            }
            else
            {
                for (int i = 0; i < bones.Length - 1; i++)
                {
                    Vector3[] pos = points;

                    Vector3 dir = (bones[i+1].position - bones[i].position).normalized;

                    dir.y = 0;

                    bones[i].up = dir;

                    points = pos;
                }
            }
        }

        public bool IsCorrectBoneCount()
        {
            return !(points.Length != bones.Length);
        }
        public void DrawGizmos()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < bones.Length; i++)
            {
                Gizmos.DrawLine(bones[i].position, bones[i].position + bones[i].up);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i], 0.1f);
            }
        }
    }
    [SerializeField] Leg rightLeg, leftLeg;
    void Start()
    {
        rightLeg.Init();
        leftLeg.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightLeg.target == null || leftLeg.target == null)
            return;
        if (!rightLeg.IsCorrectBoneCount() || !leftLeg.IsCorrectBoneCount())
        {
            rightLeg.Init();
            leftLeg.Init();
        }

        rightLeg.ResolveIK();
        leftLeg.ResolveIK();
    }

    private void OnDrawGizmos()
    {
        rightLeg.DrawGizmos();
        leftLeg.DrawGizmos();
    }
}
