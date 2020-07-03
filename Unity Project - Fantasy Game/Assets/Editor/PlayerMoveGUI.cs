using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMove))]
public class PlayerMoveGUI : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerMove move = (PlayerMove)target;
        GUILayout.Label("Velocity: " + System.Math.Round(move.velocity.magnitude, 2).ToString());
        GUILayout.Label("Grounded: " + move.GetComponent<CharacterController>().isGrounded);

        GUILayout.Space(20);
        base.OnInspectorGUI();
    }
}
