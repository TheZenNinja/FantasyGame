using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public List<Entity> entities;

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e && !entities.Contains(e))
            entities.Add(e);
    }
    private void OnTriggerExit(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e && entities.Contains(e))
            entities.Remove(e);
    }
}
