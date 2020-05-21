using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Basic")]
public class Item : ScriptableObject
{
    public ItemType type;

    public string itemName = "New Item";
    [TextArea(3,100)]
    public string discription = "I wonder what this could be?";
    public Rarity rarity = Rarity.Common;


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
            case Rarity.Celestial:
                if (!ColorUtility.TryParseHtmlString("#00dcff", out color))
                    return Color.white;
                break;
        }
        return color;
    }
    #endregion
}
