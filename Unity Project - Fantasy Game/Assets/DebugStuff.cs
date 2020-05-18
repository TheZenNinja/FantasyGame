using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{

    [Header("Time")]
    [Range(0.1f, 2f)]
    public float currentTimescale = 1;
    public float timeChangeRate = 0.1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            TimeChanging(1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            TimeChanging(-1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            TimeChanging(0);
    }
    void TimeChanging(int type)
    {
        switch (type)
        {
            case 1:
                currentTimescale += 0.1f;
                break;
            case -1:
                currentTimescale -= 0.1f;
                break;
            default:
                currentTimescale = 1;
                break;
        }
        Time.timeScale = currentTimescale;
    }
}
