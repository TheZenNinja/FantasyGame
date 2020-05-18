using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WhipBladeTester : MonoBehaviour
{
    [Range(0,0.25f)]
    public float distance = 0;
    [Range(-180, 180)]
    public float totalAngleY = 0;
    [Range(-180, 180)]
    public float totalAngleZ = 0;
    public float baseOffset = 0.1f;
    [SerializeField] Transform rootBone;
    Transform[] segments;
    LineRenderer lr;
    private void Init()
    {
        Transform[] children = rootBone.GetComponentsInChildren<Transform>();
        segments = children;

        if (lr == null)
        {
            lr = GetComponent<LineRenderer>();
            if (lr == null)
                lr = gameObject.AddComponent<LineRenderer>();
        }
        lr.positionCount = segments.Length;

    }

    private void Update()
    {

        if (segments == null || lr == null || lr.positionCount != segments.Length)
            Init();

        float angleY = totalAngleY / (segments.Length - 1);
        float angleZ = totalAngleZ / (segments.Length - 1);

        for (int i = 1; i < segments.Length; i++)
        {
            segments[i].transform.localPosition = new Vector3(0, baseOffset + distance, 0);
            segments[i].localEulerAngles = new Vector3(0, angleY, angleZ);
        }

        lr.enabled = distance > 0;

        
    }
    private void LateUpdate()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (i == segments.Length - 1)
                lr.SetPosition(i, segments[i].position - segments[i].up * baseOffset * 1.9f);
            else
                lr.SetPosition(i, segments[i].position);
        }
    }
}
