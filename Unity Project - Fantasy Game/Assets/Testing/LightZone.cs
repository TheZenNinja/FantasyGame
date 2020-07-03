using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class LightZone : MonoBehaviour
{
    public GameObject mainLight;
    public GameObject localLight;

    private void Start()
    {
        localLight.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<Camera>() == Camera.main)
        {
            mainLight.SetActive(false);
            localLight.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInChildren<Camera>() == Camera.main)
        {
            mainLight.SetActive(true);
            localLight.SetActive(false);
        }
    }
}
