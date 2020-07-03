using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/Basic")]
public class Item : ScriptableObject
{
    public bool staticInstance = false;
    [Tooltip("Item ID")]
    public int ID;
    [SerializeField]
    protected string itemName = "New Item";
    [TextArea(3,100)]
    public string description = "I wonder what this could be?";
    public Rarity rarity = Rarity.Common;
    public Sprite sprite;
    public bool stackable = true;

    public Item(string name, int ID, string discription = "", Rarity rarity = Rarity.Common, Sprite sprite = null, bool stackable = true)
    {
        this.ID = ID;
        this.itemName = name;
        this.description = discription;
        this.rarity = rarity;
        this.sprite = sprite;
        this.stackable = stackable;
    }
    public Item()
    {
        ID = 0;
        itemName = "New Item";
        //sprite = Resources.Load("Items/DefaultSprite") as Sprite;
    }

    public virtual string GetName()
    {
        return itemName;
    }

    #region Get Color
    public Color GetColor()
    {
        return GetColor(rarity);
    }

    public static Color GetColor(Rarity rarity)
    {
        Color color;
        switch (rarity)
        {
            default:
            case Rarity.Common:
                if (!ColorUtility.TryParseHtmlString("#ffffff", out color))
                    return Color.white;
                break;
            case Rarity.Uncommon:
                if (!ColorUtility.TryParseHtmlString("#1eff00", out color))
                    return Color.white;
                break;
            case Rarity.Rare:
                if (!ColorUtility.TryParseHtmlString("#0070dd", out color))
                    return Color.white;
                break;
            case Rarity.Legendary:
                if (!ColorUtility.TryParseHtmlString("#a335ee", out color))
                    return Color.white;
                break;
            case Rarity.Mystic:
                if (!ColorUtility.TryParseHtmlString("#00dcff", out color))
                    return Color.white;
                break;
        }
        return color;
    }
    #endregion

    public virtual bool SameItem(Item other)
    {
        if (this.GetType() == other.GetType())
            return itemName.CompareTo(other.itemName) == 0;//ID == other.ID;

        return false;
    }

    public virtual Item Duplicate()
    {
        Item i = new Item();
        i.ID = this.ID;
        i.itemName = this.itemName;
        i.description = this.description;
        i.rarity = this.rarity;
        i.sprite = this.sprite;
        i.stackable = this.stackable;

        return i;
    }
    /*
    public static bool CompatableTypes(ItemSlot a, ItemSlot b)
    {
        if (a.isOutput && b.item != null)
            return false;
        if (b.isOutput && a.item != null)
            return false;

        bool c = a.item == null || CompatableTypes(b.type, a.item.type); 
        bool d = b.item == null || CompatableTypes(a.type, b.item.type);
        return c & d;
    }

    public static bool CompatableTypes(ItemType slot, ItemType item)
    {
        if (slot == item)
            return true;
        if (slot == ItemType.none)
            return true;

        switch (item)
        {
            case ItemType.blade:
            case ItemType.handle:
            case ItemType.core:
            case ItemType.crystal:
            case ItemType.harness:
            case ItemType.focus:
                if (slot == ItemType.weaponPart)
                    return true;
                break;
            
            case ItemType.ore:
            case ItemType.ingot:
                if (slot == ItemType.material)
                    return true;
                break;
        }

        return false;
    }*/
}