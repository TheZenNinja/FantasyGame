using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crystal")]
public class CrystalItem : Item
{
    public CrystalItem()
    { type = ItemType.crystal; }
    public Element element;

    [Tooltip("Dialouge for each rarity")]
    public string[] lines;

    #region Get Rarity Name
    public string GetTypeName()
    {
        return GetTypeName(rarity);
    }

    public static string GetTypeName(Rarity rarity)
    {
        switch (rarity)
        {
            default:
            case Rarity.Common:
                return "Inert";
            case Rarity.Uncommon:
                return "Codex";
            case Rarity.Rare:
                return "Inscribed";
            case Rarity.Legendary:
                return "Wispering";
            case Rarity.Celestial:
                return "Celestial";
        }
    }
    #endregion
}
