﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}