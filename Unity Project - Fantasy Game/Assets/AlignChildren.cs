using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignChildren : MonoBehaviour
{
    [ContextMenu("Align")]
    public void Align()
    {
        foreach (Transform t in transform)
        {
            float[] pos = { t.localPosition.x, t.localPosition.y, t.localPosition.z };

            for (int i = 0; i < pos.Length; i++)
                pos[i] = Mathf.RoundToInt(pos[i]);

            t.localPosition = new Vector3(pos[0], pos[1], pos[2]);
        }
    }
}
