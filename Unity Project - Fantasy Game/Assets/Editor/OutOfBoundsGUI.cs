using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OutOfBoundsTest))]
public class OutOfBoundsGUI : Editor
{

    private void OnSceneGUI()
    {
        var oob = (OutOfBoundsTest)target;

        Handles.color = Color.white;
        Handles.DrawWireDisc(oob.transform.position, oob.transform.up, oob.maxDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(oob.returnPos, 0.25f);
    }

}
