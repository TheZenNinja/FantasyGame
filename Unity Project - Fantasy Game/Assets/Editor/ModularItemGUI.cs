using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ModularItem))]
public class ModularItemGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(25);
        ModularItem item = target as ModularItem;
        if (GUILayout.Button("Update Data"))
            item.UpdateData();
    }
}
