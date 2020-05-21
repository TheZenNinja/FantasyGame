using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Parts/Basic Part")]
public class WeaponPart : Item
{
    [Tooltip("0.1, 0.2, 0.25, 0.5, 0.75, 1")]
    public float materialEffiency = 1;
    public PartMaterial partMaterial;
}
