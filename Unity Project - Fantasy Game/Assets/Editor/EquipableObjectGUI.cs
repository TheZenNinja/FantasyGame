using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EquipableObject))]
public class ToolObjectGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(25);

        EquipableObject assembler = (EquipableObject)target;
        GUILayout.Space(25);
        if (GUILayout.Button("Assemble", GUILayout.Height(25)))
            assembler.Assemble();
    }
}
