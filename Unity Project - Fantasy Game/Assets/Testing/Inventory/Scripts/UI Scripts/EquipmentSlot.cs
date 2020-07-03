using UnityEngine;
using System.Collections;

public class EquipmentSlot : ItemSlot
{
    [SerializeField] EquipmentManager equipment;
    [SerializeField] int index;

    public void Awake()
    {
        OnDataChange += UpdateUI;
        OnDataChange += UpdateTool;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        
    }
    public void UpdateTool()
    {
        if (hasItem())
        {
            if (item.GetType() == typeof(ModularItem))
                equipment.SetHotbarSlot(item as ModularItem, index);
        }
        else
        {
            equipment.SetHotbarSlot(null, index);
        }
    }
}
