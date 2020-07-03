using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ModularItem : Item
{
    public EquipmentType equipmentType;
    public WeaponType weaponType;
    //public ToolType toolType;
    public List<Part> parts;
    public int itemTier = 0;
    public float swingRange = 1.5f;
    public int attackDmg = 10;

    public bool canDisassemble = true;

    public ModularItem(Part[] parts, WeaponType type, Sprite sprite = null, bool canDisassemble = true)
    {
        Init(new List<Part>(parts), type, sprite, canDisassemble);
    }
    public ModularItem(List<Part> parts, WeaponType type, Sprite sprite = null, bool canDisassemble = true)
    {
        Init(parts, type, sprite, canDisassemble);
    }
    private void Init(List<Part> parts, WeaponType type, Sprite sprite = null, bool canDisassemble = true)
    {
        stackable = false;
        this.parts = parts;
        this.canDisassemble = canDisassemble;
        weaponType = type;
        if (sprite != null)
            this.sprite = sprite;


        UpdateData();
    }

    public void UpdateData()
    {
        foreach (Part p in parts)
            if (PartAtlas.GetEquipmentType(p.type) != EquipmentType.none)
            {
                if (sprite == null)
                    sprite = ItemSpriteAtlas.instance.GetModularSprite(p.type, weaponType);

                equipmentType = PartAtlas.GetEquipmentType(p.type);

                if (weaponType == WeaponType.none)
                    itemName = UsefulFunctions.Capitalize(PartMaterialAtlas.GetMaterialName(p.material) + " " + PartAtlas.GetToolName(p.type));
                else
                    itemName = UsefulFunctions.Capitalize(PartMaterialAtlas.GetMaterialName(p.material) + " " + WeaponAtlas.GetName(weaponType));


                if (weaponType != WeaponType.none)
                    description = "Weapon Type: " + WeaponAtlas.GetName(weaponType);
                else
                    description = "Type: " + PartAtlas.GetToolName(p.type);

                description += "\nMaterial: " + PartMaterialAtlas.GetMaterialName(p.material);

                attackDmg = WeaponAtlas.CalulateDamage(weaponType, p.material);
                description += "\nDamage: " + attackDmg;

                break;
            }

    }

    public List<Part> GetPartList()
    {
        return parts;
    }
}



public enum EquipmentType
{
    none,
    tool,
    sword,
    bow,
    gun,
}
public enum WeaponType
{
    none,
    //sword
    onehanded,
    twohanded,
    katana,
    rapier,
}
/*public enum ToolType
{
    none,
    pick,
    axe,
    hammer
}*/
