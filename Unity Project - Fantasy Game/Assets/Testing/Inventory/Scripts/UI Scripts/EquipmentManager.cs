using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class EquipmentManager : MonoBehaviour
{
    public FirstPersonRigTest animData;
    public GameObject bg;
    public List<ModularItem> items;
    public List<EquipableObject> equipObjs;
    public List<EquipmentSlot> itemSlots;
    public List<HotbarSlot> hotbarSlots;
    public int currentToolIndex;

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemSlots[i].SetItem(items[i]);
        }

        UpdateHotbar();
    }

    public void SetHotbarSlot(ModularItem item, int index)
    {
        items[index] = item;
        UpdateHotbar();
    }

    public void SetItems(List<ModularItem> newItems)
    {
        this.items = newItems;
        for (int i = 0; i < newItems.Count; i++)
        {
            equipObjs[i].item = newItems[i];
            itemSlots[i].SetItem(newItems[i]);
            hotbarSlots[i].SetItem(newItems[i]);
        }
        UpdateHotbar();
    }
    public List<ModularItem> GetHotbar()
    {
        return items;
    }

    public ModularItem GetCurrentItem()
    {
        if (currentToolIndex > 0)
            return equipObjs[currentToolIndex - 1].item;
        else
            return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentToolIndex == 1)
                currentToolIndex = 0;
            else
                currentToolIndex = 1;
            UpdateHotbar();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentToolIndex == 2)
                currentToolIndex = 0;
            else
                currentToolIndex = 2;
            UpdateHotbar();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentToolIndex == 3)
                currentToolIndex = 0;
            else
                currentToolIndex = 3;
            UpdateHotbar();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (currentToolIndex == 4)
                currentToolIndex = 0;
            else
                currentToolIndex = 4;
            UpdateHotbar();
        }
    }

    public void UpdateHotbar()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            equipObjs[i].item = items[i];

            hotbarSlots[i].SetItem(items[i]);

            equipObjs[i].Assemble();
        }

        EquipItem();
    }
    public void EquipItem()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (equipObjs[i].mesh)
            {
                if (currentToolIndex-1 == i)
                    equipObjs[i].Equip();
                else
                    equipObjs[i].Unequip();
            }
            if (currentToolIndex-1 == i)
                hotbarSlots[i].SwitchHighlight(true);
            else
                hotbarSlots[i].SwitchHighlight(false);
        }
        if (currentToolIndex > 0)
            animData.SwitchItem(items[currentToolIndex - 1]);
        else
            animData.SwitchItem(null);
    }

    public void Open()
    {
        bg.SetActive(true);
    }

    internal void Close()
    {
        bg.SetActive(false);
    }
}
