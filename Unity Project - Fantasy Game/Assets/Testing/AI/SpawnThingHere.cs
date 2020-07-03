using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThingHere : MonoBehaviour
{
    public GameObject prefab;

    public void SpawnThing()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
