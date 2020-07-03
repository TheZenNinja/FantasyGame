using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class UsefulFunctions
{
    public static Vector3 V3ZeroY(Vector3 input)
    {
        return new Vector3(input.x, 0, input.z);
    }

    private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
    public static string Capitalize(string input)
    {
        //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        return textInfo.ToTitleCase(input);
    }
}
