using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;

    public List<ItemSaveData> equipment;
    public List<ItemSaveData> items;


    public List<ModularItem> GetEquipment()
    {
        List<ModularItem> loadData = new List<ModularItem>();
        foreach (ItemSaveData item in equipment)
        {
            if (item.emptySlot)
                loadData.Add(null);
            else
                loadData.Add(item.ToItem() as ModularItem);
        }
        return loadData;
    }

    public void SetEquipment(List<ModularItem> newTools)
    {
        List<ItemSaveData> saveData = new List<ItemSaveData>();
        foreach (ModularItem item in newTools)
        {
            if (item == null)
                saveData.Add(new ItemSaveData());
            else
                saveData.Add(new ItemSaveData(item));
        }
        equipment = saveData;
    }
    public List<InventorySlot> GetItems()
    {
        List<InventorySlot> newItems = new List<InventorySlot>();

        foreach (ItemSaveData itemData in items)
        {
            if (itemData.emptySlot)
                newItems.Add(new InventorySlot(null, 0));
            else
            {
                Item item;
                switch (itemData.itemType)
                {
                    //generic = 0, material = 1: , part = 2, modular = 3
                    default:
                        item = itemData.ToItem();
                    break;
                    case 1:
                        item = itemData.ToItem() as MaterialItem;
                        break;
                    case 2:
                        item = itemData.ToItem() as PartItem;
                        break;
                    case 3:
                        item = itemData.ToItem() as ModularItem;
                        break;
                }
                newItems.Add(new InventorySlot(item, itemData.itemCount));
            }
        }

        return newItems;
    }
    public void SetItems(List<InventorySlot> newItems)
    {
        List<ItemSaveData> saveData = new List<ItemSaveData>();
        foreach (InventorySlot inv in newItems)
        {
            if (inv.item== null)
                saveData.Add(new ItemSaveData());
            else
                saveData.Add(new ItemSaveData(inv));
        }
        items = saveData;
    }
    public void SetItems(List<Item> items, List<int> counts)
    {
        List<InventorySlot> invList = new List<InventorySlot>();

        for (int i = 0; i < items.Count; i++)
            invList.Add(new InventorySlot( items[i], counts[i]));

        SetItems(invList);
    }
}