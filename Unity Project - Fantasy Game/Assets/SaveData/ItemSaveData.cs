using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ItemSaveData
{
    public int itemCount;
    public bool emptySlot = false;
    public Item staticItem;
    //generic = 0, material = 1, part = 2, modular = 3
    public int itemType;

    //generic
    public string itemName;
    public Sprite itemSprite;
    //material
    public ResourceType resourceType;
    public PartMaterial resourceMaterial;
    //part
    //modular
    public List<Part> parts;
    public WeaponType weaponType;
    public bool canDisassemble;

    /*public void SetBasicData(Item item, int count)
    {
        itemCount = count;
            itemName = item.GetName();
    }*/
    public void SetBasicData(Item item, int count)
    {
        if (count == 0 || item == null)
        {
            emptySlot = true;
            return;
        }

        if (item.GetType() == typeof(ModularItem))
            itemType = 3;
        else if (item.GetType() == typeof(PartItem))
            itemType = 2;
        else if (item.GetType() == typeof(MaterialItem))
            itemType = 1;
        else
            itemType = 0;


        itemCount = count;
        if (item.staticInstance)
        {
            staticItem = item;
            return;
        }

        itemName = item.GetName();
        itemSprite = item.sprite;

        switch (itemType)
        {
            case 3:
                {
                    ModularItem modularItem = item as ModularItem;
                    parts = modularItem.parts;
                    canDisassemble = modularItem.canDisassemble;
                    weaponType = modularItem.weaponType;
                }
                break;
            case 2:
                {
                    PartItem partItem = item as PartItem;
                    parts = new List<Part>() { partItem.part };
                }
                break;
            case 1:
                {
                    MaterialItem materialItem = item as MaterialItem;
                    resourceMaterial = materialItem.material;
                    resourceType = materialItem.resourceType;
                }
                break;
            default:
                break;
        }
    }
    public ItemSaveData(InventorySlot slot)
    {
        SetBasicData(slot.item, slot.count);
    }

    public ItemSaveData(Item item, int count = 1)
    {
        SetBasicData(item, count);
    }

    public ItemSaveData()
    {
        emptySlot = true;
    }

    /*public ItemSaveData(Item item, int count = 1)
    {
        itemType = 0;
        SetBasicData(item, count);
    }

    public ItemSaveData(MaterialItem item, int count = 1)
    {
        itemType = 1;
        SetBasicData(item, count);
        {
            resourceType = item.resourceType;
            partMaterial = item.material;
        }
    }
    public ItemSaveData(ModularItem item, int count = 1)
    {
        itemType = 4;
        SetBasicData(item, count);
        {
            parts = item.parts;
        }
    }*/

    public Item ToItem()
    {
        if (emptySlot)
            return null;

        if (staticItem)
        {
            return staticItem;
        }

        switch (itemType)
        {
            case 0:
            default:
                return new Item(itemName, 0, sprite: itemSprite);
            case 1:
                return new MaterialItem(resourceType, resourceMaterial);
            case 2:
                return new PartItem(parts[0], itemSprite);
            case 3:
                return new ModularItem(parts, weaponType, itemSprite, canDisassemble);
        }
    }
}
