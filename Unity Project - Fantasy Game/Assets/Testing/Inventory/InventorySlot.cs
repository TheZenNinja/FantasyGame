using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Item _i;
    public Item item
    {
        get
        {
            return _i;
        }
        set
        {
            _i = value;
            UpdateUI();
        }

    }
    [SerializeField] Image rarityBG;
    [SerializeField] Image sprite;




    public void UpdateUI()
    {
        if (item)
        {
            sprite.enabled = true;
            sprite.sprite = item.sprite;
            rarityBG.color = Item.GetColor(item.rarity);
        }
        else
        {
            sprite.enabled = false;
            rarityBG.color = new Color(0.8f, 0.8f, 0.8f);
        }
    }


    private void OnValidate()
    {
        UpdateUI();
    }
}
