using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class OutOfBoundsTest : MonoBehaviour
{
    public float minY = -50, maxY = 200;
    public float maxDistance = 100;

    //public List<Transform> objectsToTrack;
    public Transform player;


    public Vector3 returnPos;

    void Start()
    {
        InvokeRepeating(nameof(TestDistance), 10, 10);
    }

    public void TestDistance()
    {
        if (player.position.y > maxY ||
            player.position.y < minY ||
            (player.position - transform.position).sqrMagnitude > maxDistance * maxDistance)
        {
            Debug.Log("Teleporting player back into bounds");
            player.GetComponent<CharacterController>().enabled = false;
            player.position = transform.TransformPoint(returnPos);
            player.GetComponent<PlayerMove>().velocity = Vector3.zero;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
