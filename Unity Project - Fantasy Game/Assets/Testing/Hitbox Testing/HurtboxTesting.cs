using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxTesting : MonoBehaviour
{
    Renderer renderer;
    public Material green, red;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        TurnGreen();
    }
    public void Hit()
    {
        renderer.material = red;
        if (IsInvoking(nameof(TurnGreen)))
            CancelInvoke(nameof(TurnGreen));
        Invoke(nameof(TurnGreen), 1);
    }
    void TurnGreen()
    {
        renderer.material = green;
    }
}
