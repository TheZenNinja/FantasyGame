using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : EntityStatus
{
    public static PlayerStatus instance;
    public PlayerStatus()
    {
        instance = this;
    }

    FirstPersonRigTest test;
    PlayerMove move;
    Health health;
    public AudioSource hitSound;

    private void Start()
    {
        move = GetComponent<PlayerMove>();
        health = GetComponent<Health>();
        health.Hurt += OnHit;
        test = GetComponentInChildren<FirstPersonRigTest>();
    }

    public Health GetHealth() => health;

    public void OnHit(int dontBother) => hitSound?.Play();
}
