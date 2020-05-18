using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    public Vector3 hitboxSizeMulti = Vector3.one;

    public Vector3 hitboxSize = Vector3.one;

    public Vector3 hitboxStartPos;

    public BoxCollider col;

    public bool breaker, lifter;

    public bool canDamage;

    void FixedUpdate()
    {
        UpdateLength();
    }

    public void UpdateLength()
    {
        Vector3 realSize = Vector3.Scale(hitboxSize, hitboxSizeMulti);


        Vector3 offset = Vector3.zero;
        offset.y = realSize.y / 2;
        offset += hitboxStartPos;

        col.size = realSize;
        col.center = offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage)
            return;

        Entity e = other.GetComponent<Entity>();
        if (e != null && e != Attacking.instance.currentEntity)
            e.Hit();
    }

    private void OnValidate()
    {
        UpdateLength();
    }
}
