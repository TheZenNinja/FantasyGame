using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuilder : MonoBehaviour
{
    #region Singleton
    public static ArmorBuilder instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public bool highlightAreas;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            highlightAreas = !highlightAreas;
    }
}
