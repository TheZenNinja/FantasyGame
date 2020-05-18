﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    #region Singleton
    public static AnimationEvents instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion



    public void Hit()
    {
        Attacking.instance.Hit();
    }

    public void ToggleHitbox(bool active)
    {
        Attacking.instance.ToggleParticle(active);

        if (active)
            Debug.Log("Hitbox enabled");
        else
            Debug.Log("Hitbox disabled");
    }
    public void EnableHitbox()
    {
        ToggleHitbox(true);
    }
    public void DisableHitbox()
    {
        ToggleHitbox(false);
    }
    public void AllowInterrupt()
    {
        Attacking.instance.CanInterrupt = true;
    }
    public void Launch(Vector3 dir)
    {
        if (dir == Vector3.zero)
            dir = Vector3.up;

        Attacking.instance.LaunchTarget(dir);
    }
}