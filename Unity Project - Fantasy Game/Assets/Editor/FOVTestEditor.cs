using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FOVTest))]
public class FOVTestEditor : Editor
{
    private void OnSceneGUI()
    {
        FOVTest fov = (FOVTest)target;
        Vector3 center = fov.transform.position;
        Handles.color = Color.white;
        Handles.DrawWireArc(center, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2);
        Handles.DrawLine(center, center + viewAngleA * fov.viewRadius);
        Handles.DrawLine(center, center + viewAngleB * fov.viewRadius);
    }
}
