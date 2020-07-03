using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Singletons/Item Sprite Atlas")]

public class ItemSpriteAtlas : ScriptableObject
{
    #region Singleton
    public static ItemSpriteAtlas instance
    {
        get
        {
            return Resources.Load<ItemSpriteAtlas>("Atlas/Item Sprite Atlas");
        }
    }
    #endregion

    public Sprite ingot, ore, wood, stone;
    [Space]
    public List<Sprite> parts;
    public List<Sprite> swords;
    [Space]
    public Sprite axe;
    public Sprite pick, hammer;
    [Space]
    public Sprite gear;

    public Sprite GetResourceSprite(ResourceType type)
    {
        switch (type)
        {
            default:
            case ResourceType.ingot:
                return ingot;
            case ResourceType.ore:
                return ore;
            case ResourceType.wood:
                return wood;
            case ResourceType.stone:
                return stone;
        }
    }

    public Sprite GetPartSprite(PartType type)//, int subtype)
    {
        return parts[(int)type];
    }
    public Sprite GetModularSprite(PartType type, WeaponType weaponType)
    {
        switch (weaponType)
        {
            default:
            case WeaponType.none:
                switch (type)
                {
                    case PartType.axeHead:
                        return axe;
                    case PartType.pickHead:
                        return pick;
                    case PartType.hammerHead:
                        return hammer;
                    default:
                        return null;
                }
            case WeaponType.onehanded:
                return swords[0];
            case WeaponType.katana:
                return swords[1];
            case WeaponType.rapier:
                return swords[2];
            case WeaponType.twohanded:
                return swords[3];
        }
    }
}