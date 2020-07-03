using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageMenu : MonoBehaviour
{
    //public SavedInventory savedInventory;

    public int slotCount = 12;
    public List<Item> items;
    public List<int> itemCount;
    //add a list of ints for the item count
    //create player inventory class?
    public List<ItemSlot> slots;

    public void SetItems(List<InventorySlot> invSlots)
    {
        slotCount = invSlots.Count;
        Initialize();
        

        for (int i = 0; i < invSlots.Count; i++)
        {
            items[i] = invSlots[i].item;
            itemCount[i] = invSlots[i].count;
        }

    }

    public Transform grid;
    public GameObject bg;
    public bool isOpen;

    private void Start()
    {
        /*if (savedInventory)
        {
            LoadInventory();
        }
        else*/
        {
            Initialize();
        }
        isOpen = false;
        bg.SetActive(false);
    }
    public void Initialize()
    {
        if (slotCount < items.Count)
            slotCount = items.Count;
        else if (slotCount > items.Count)
            for (int i = 0; i < slotCount; i++)
                if (i >= items.Count)
                    items.Add(null);

        if (slotCount < itemCount.Count)
            slotCount = itemCount.Count;
        else if (slotCount > itemCount.Count)
            for (int i = 0; i < slotCount; i++)
                if (i >= itemCount.Count)
                {
                    if (items[i])
                        itemCount.Add(1);
                    else
                        itemCount.Add(0);
                }
    }
    /*public void LoadInventory()
    {
        if (!savedInventory)
            return;
        slotCount = savedInventory.items.Count;
        Initialize();

        for (int i = 0; i < slotCount; i++)
        {
            items[i] = savedInventory.items[i].item;
            itemCount[i] = savedInventory.items[i].count;
        }
    }

    public void SaveInventory()
    {
        if (!savedInventory)
            return;
        savedInventory.items = new List<InventorySlot>();

        for (int i = 0; i < slotCount; i++)
        {
            savedInventory.items.Add(new InventorySlot(items[i], itemCount[i]));
        }
    }*/

    public void Open()
    {
        if (isOpen)
            return;
        isOpen = true;
        bg.SetActive(true);
        //if (savedInventory)
          //  LoadInventory();
        if (slots.Count != slotCount)
            GenerateSlots();
        SyncSlotsToInventory();
    }

    public void Close()
    {
        if (!isOpen)
            return;
        isOpen = false;
        SyncInventoryToSlots();
        //if (savedInventory)
          //  SaveInventory();
        bg.SetActive(false);
    }

    public void GenerateSlots()
    {
        if (slots.Count > 0)
        {
            foreach (ItemSlot slot in slots)
            {
                if (Application.isEditor)
                    DestroyImmediate(slot.gameObject);
                else
                    Destroy(slot.gameObject);
            }
            slots.Clear();
        }
        GameObject slotPref = Resources.Load("Inventory Slot", typeof(GameObject)) as GameObject;

        if (slots.Count > 0)
            for (int i = 0; i < slots.Count; i++)
                Destroy(slots[i].gameObject);

        for (int i = 0; i < slotCount; i++)
        {
            GameObject g = Instantiate(slotPref, grid);
            g.name = "Slot " + (i + 1);
            slots.Add(g.GetComponent<ItemSlot>());
        }
    }

    public void SyncSlotsToInventory()
    {
        for (int i = 0; i < slotCount; i++)
        {
            slots[i].SetItem(items[i], itemCount[i]);
        }
    }
    public void SyncInventoryToSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            items[i] = slots[i].GetItem();
            itemCount[i] = slots[i].GetCount();
        }
    }
    public void TryQuickStack(ItemSlot slot)
    {
        throw new System.NotImplementedException();
    }

    public bool IsFull()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (items[i] == null)
                return false;
        }
        return true;
    }

    public bool ContainsItem(Item itemToFind)
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (items[i].SameItem(itemToFind))
                return true;
        }
        return false;
    }

    public bool AddItem(Item item, int count = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i])
            {
                if (item.GetType() == items[i].GetType() && item.stackable && items[i].SameItem(item))
                {
                    itemCount[i] += count;
                    if (isOpen)
                        SyncSlotsToInventory();
                    return true;
                }
            }
            else
            {
                items[i] = item;
                itemCount[i] = count;
                if (isOpen)
                    SyncSlotsToInventory();
                return true;
            }
        }
        return false;
    }
}
