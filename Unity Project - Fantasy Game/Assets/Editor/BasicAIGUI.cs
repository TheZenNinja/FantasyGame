using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicAI))]
public class BasicAIGUI : Editor
{
    private void OnSceneGUI()
    {
        BasicAI ai = (BasicAI)target;

        Vector3 pos = ai.transform.position;

        Handles.color = Color.red;
        Handles.DrawWireDisc(pos + ai.transform.up, Vector3.up, ai.attackDistance);

        Handles.color = Color.white;
        if (ai.fovLayers.Length > 0)
            foreach (FOV fov in ai.fovLayers)
            {
                if (fov.angle >= 360)
                {
                    Handles.DrawWireDisc(ai.transform.position + ai.transform.up, Vector3.up, fov.distance);
                }
                else
                {
                    Vector3 angleA = ai.DirFromAngle(-fov.angle / 2);
                    Vector3 angleB = ai.DirFromAngle(fov.angle / 2);

                    Handles.DrawWireArc(pos + ai.transform.up, Vector3.up, angleA, fov.angle, fov.distance);

                    Handles.DrawLine(pos + ai.transform.up, pos + ai.transform.up + angleA * fov.distance);
                    Handles.DrawLine(pos + ai.transform.up, pos + ai.transform.up + angleB * fov.distance);
                }
            }
    }
}
