using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Parts/Core")]
public class CoreItem : Item
{
    public CoreItem()
    { type = ItemType.core; }
    public CrystalItem crystal;

    public PartMaterial harness;
    [HideInInspector]
    public static float harnessEffiency = 1;

    public WeaponPart[] foci;

    public GameObject prefab;
}
