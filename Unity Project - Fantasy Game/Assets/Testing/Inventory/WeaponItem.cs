using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public class PartAttribute
    {
        public MaterialAttribute attribute;
        public float multi;

        public static PartAttribute operator +(PartAttribute a, PartAttribute b)
        {
            if (a.attribute == b.attribute)
            {
                a.multi += b.multi;
                return a;
            }
            else
                throw new System.InvalidOperationException("Attributes don't match");
        }
        public static PartAttribute operator -(PartAttribute a, PartAttribute b)
        {
            if (a.attribute == b.attribute)
            {
                a.multi -= b.multi;
                if (a.multi < 0)
                {
                    a.multi = 0;
                    Debug.LogError("Attributes cannot go below 0, rebalnce perks");
                }
                return a;

            }
            else
                throw new System.InvalidOperationException("Attributes don't match");
        }
    }

    public WeaponItem()
    { type = ItemType.weapon; }

    public WeaponPart blade;
    public HandleItem handle;
    public CoreItem core;
    public WeaponPart other;
}
