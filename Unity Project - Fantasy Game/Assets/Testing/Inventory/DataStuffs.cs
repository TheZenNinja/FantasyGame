using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common, //white 	    #ffffff
    Uncommon, //green	    #1eff00
    Rare, //dark blue	    #0070dd
    Legendary, // purple	#a335ee
    Celestial //cyan        #00dcff
    //alternative: orange   #ff8000
}

public enum Element
{
    Fire,
    Water,
    Earth,
    Air,
    Lightning,
}

public enum PartMaterial
{
    //T1
    wood,
    stone,
    copper,
    //T2
    Silverwood, // 2x mana eff
    bronze,
    Iron,
    Invar,
    Osmium,
    //T3
    obsidian,
    manaStone,
    silver, //- anti monster
    steel,
    //T4
    Damascus,
    cobalt, //- 2x mana dmg
    manaSteel, //- 2x mana efficiency
    shadowSteel, //- magic resistant
    Platinum,
    //Elemental (T5)
    mithril, //- air
    electrum, //- lightning
    adamantite, //- earth
    neptunium, //- water
    vulcanium, //- fire
    //Celestial (T6, Hidden)
    nullSteel, //- anti magic
    quickSilver, //- anti monster
    IchorSteel, //- anti god
    worldTree, //- self repairing
    CelestialQuartz, //- 2x all affinities
    SoulSteel //- 2x soul drop chance, 2x soul drop rarity
}

public enum MaterialAttribute
{
    none,
    manaEff,
    manaDmg,
    antiMonster,
    manaResist,
    elemental
}

public enum ItemType
{
    none,
    weapon,
    //parts
    blade,
    handle,
        guard,
        wrap,
    core,
        crystal,
        harness,
        focus,
}

public static class ElementalFunctions
{
    private static float matchupModifier = 0.25f;

    public static float GetElementMatchupMulti(Element attackingElement, Element targetElement)
    {
        float matchup = 1f;

        if (attackingElement == targetElement)
            return 1;

        switch (attackingElement)
        {
            case Element.Fire:
                if (targetElement == Element.Air)
                    matchup += matchupModifier;
                else if (targetElement == Element.Water)
                    matchup -= matchupModifier;
                break;
            case Element.Water:
                if (targetElement == Element.Fire)
                    matchup += matchupModifier;
                else if (targetElement == Element.Lightning)
                    matchup -= matchupModifier;
                break;
            case Element.Earth:
                if (targetElement == Element.Lightning)
                    matchup += matchupModifier;
                else if (targetElement == Element.Air)
                    matchup -= matchupModifier;
                break;
            case Element.Air:
                if (targetElement == Element.Earth)
                    matchup += matchupModifier;
                else if (targetElement == Element.Fire)
                    matchup -= matchupModifier;
                break;
            case Element.Lightning:
                if (targetElement == Element.Water)
                    matchup += matchupModifier;
                else if (targetElement == Element.Earth)
                    matchup -= matchupModifier;
                break;
        }
        return matchup;
    }
}

public static class PartMaterialAtlas
{
    public static MaterialAttribute GetMaterialAttribute(PartMaterial mat)
    {
        switch (mat)
        {
            default:
            case PartMaterial.wood:
            case PartMaterial.stone:
            case PartMaterial.copper:
            case PartMaterial.bronze:
            case PartMaterial.Iron:
            case PartMaterial.Invar:
            case PartMaterial.Osmium:
            case PartMaterial.obsidian:
            case PartMaterial.steel:
            case PartMaterial.Damascus:
            case PartMaterial.Platinum:
                return MaterialAttribute.none;
            case PartMaterial.Silverwood:
            case PartMaterial.manaStone:
            case PartMaterial.manaSteel:
                return MaterialAttribute.manaEff;
            case PartMaterial.silver:
                return MaterialAttribute.antiMonster;
            case PartMaterial.cobalt:
                return MaterialAttribute.manaDmg;
            case PartMaterial.shadowSteel:
                return MaterialAttribute.manaResist;
            case PartMaterial.mithril:
            case PartMaterial.electrum:
            case PartMaterial.adamantite:
            case PartMaterial.neptunium:
            case PartMaterial.vulcanium:
                return MaterialAttribute.elemental;
            /*break;    
            case PartMaterial.quickSilver:
                break;    
            case PartMaterial.IchorSteel:
                break;    
            case PartMaterial.worldTree:
                break;    
            case PartMaterial.CelestialQuartz:
                break;
            case PartMaterial.SoulSteel:
                    break;*/
        }
    }
}
