using System;
using UnityEngine;
using System.Collections.Generic;

public class HealthPickup : MonoBehaviour, IPickup
{
    public int healAmt = 5;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatus>())
            if (TryPickup(other.GetComponent<PlayerStatus>()))
                Destroy(gameObject);
    }
    public bool TryPickup(EntityStatus entity)
    {
        if (entity.GetType() == typeof(PlayerStatus))
        {
            PlayerStatus player = entity as PlayerStatus;
            if (player.GetHealth().Heal(healAmt))
                return true;
        }
        return false;
    }
}
