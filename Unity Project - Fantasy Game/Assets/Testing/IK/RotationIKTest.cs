using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RotationIKTest : MonoBehaviour
{
    public Transform one, two, pole;

    public Transform pos;

    void Update()
    {
        two.position = pos.position;
        Vector3 oneGlobal = one.position, twoGlobal = two.position;

        Vector3 dir = (one.position - two.position).normalized;

        one.up = dir;

        one.position = oneGlobal;
        two.position = twoGlobal;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(one.position, one.position - one.up);
        Gizmos.DrawLine(two.position, two.position - two.up);

        Vector3 dir = Vector3.zero;
        dir = (one.position - two.position).normalized;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(one.position, one.position - dir);
    }
}
