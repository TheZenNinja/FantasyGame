using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    public Faction faction;
    bool hitboxActive = false;
    List<IDamagableObject> objs;
    public BoxCollider hitbox;
    public bool isPlayer;

    public void Hit(int damage = 1)
    {
        Bounds bounds = hitbox.bounds;
        Collider[] hits = Physics.OverlapBox(bounds.center, bounds.extents);

        objs = new List<IDamagableObject>();
        foreach (Collider c in hits)
        {
            IDamagableObject obj;
            if (c.TryGetComponent(out obj))
            {
                Health h;
                if (c.TryGetComponent(out h) && h.faction == faction)
                    continue;
                else
                {
                    if (!objs.Contains(obj))
                        objs.Add(obj);
                }
            }
        }

        foreach (IDamagableObject obj in objs)
        {
            obj.DamageObj(damage, GetComponentInParent<EntityStatus>());
        }
    }

    public void SetActive(bool active)
    {
        hitboxActive = active;
    }
}
