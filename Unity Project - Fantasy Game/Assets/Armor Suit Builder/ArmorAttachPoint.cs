using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAttachPoint : MonoBehaviour
{
    public ArmorPart childPart;
    public GameObject highlightObj;

    private void Update()
    {
        if (highlightObj)
            highlightObj.SetActive(ArmorBuilder.instance.highlightAreas);
    }
}
