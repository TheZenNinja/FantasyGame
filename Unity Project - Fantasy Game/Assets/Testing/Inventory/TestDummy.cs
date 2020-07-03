using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamagableObject
{
    public Transform damagePos;

    private AudioSource source;
    void Start() => source = GetComponentInChildren<AudioSource>();

    public void DamageObj(int dmg, EntityStatus sender)
    {
        //Debug.Log(dmg);
        source.Play();
        DamagePopup.Create(dmg, damagePos.position, 0.5f);
    }
}
