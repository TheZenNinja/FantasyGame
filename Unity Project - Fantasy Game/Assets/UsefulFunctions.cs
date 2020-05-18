using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulFunctions
{
    public static Vector3 V3ZeroY(Vector3 input)
    {
        return new Vector3(input.x, 0, input.z);
    }
}
