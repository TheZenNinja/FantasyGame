using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Weapon Parts/Handle")]

public class HandleItem : Item
{
    public HandleItem()
    {
        type = ItemType.handle;
    }

    public PartMaterial baseMaterial;
    public PartMaterial accentMaterial;

    public static float baseMaterialEffiency = 0.5f;
}
