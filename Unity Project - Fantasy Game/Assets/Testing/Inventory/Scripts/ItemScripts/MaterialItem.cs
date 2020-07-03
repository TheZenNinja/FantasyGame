using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum ResourceType
{
    ingot, ore, wood, stone
}
[CreateAssetMenu]
public class MaterialItem : Item
{
    public ResourceType resourceType;
    public PartMaterial material;

    public MaterialItem(ResourceType resourceType, PartMaterial material, bool useSuffix = true)
    {
        this.resourceType = resourceType;
        this.material = material;

        sprite = ItemSpriteAtlas.instance.GetResourceSprite(resourceType);
        rarity = (Rarity)PartMaterialAtlas.GetMaterialData(material).materialLevel - 1;

        itemName = PartMaterialAtlas.GetMaterialName(material);

        if (PartMaterialAtlas.MaterialUsesSuffix(material))
            itemName += " " + UsefulFunctions.Capitalize(resourceType.ToString());
    }

    public override bool SameItem(Item other)
    {
        if (typeof(MaterialItem) == other.GetType())
        {
            MaterialItem m = other as MaterialItem;
            if (material == m.material)
                return true;
        }
        return false;
    }

    public override Item Duplicate()
    {
        MaterialItem i = new MaterialItem(resourceType, material);
        return i;
    }

}