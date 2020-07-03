using System;
using System.Collections.Generic;
using UnityEngine;
public class TapHoldInput
{
    public KeyCode key;
    public float holdTime = 0.25f;
    public Action tap, hold, release;

    public void Press()
    {
        //invoke(nameof(TestHold), holdTime);
    }
    public void Release()
    {

    }
    private void TestHold()
    {

    }
}
