using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common, //white 	    #ffffff
    Uncommon, //green	    #1eff00
    Rare, //dark blue	    #0070dd
    Legendary, // purple	#a335ee
    Mystic //cyan        #00dcff
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
    none,

    //T1
    wood,
    stone,
    aluminium, //light II
    copper,
    //T2
    silverwood, // mana eff
    iron,
    bronze, //Light I
    //invar,
    osmium, //heavy 1
    //T3
    obsidian,
    manaStone,
    silver, //- anti monster I
    steel,
    //T4
    damascus,
    cobalt, //- mana dmg
    manaSteel, //- mana efficiency
    shadowSteel, //- magic resistant 
    platinum, //light
    //Elemental (T5)
    mythril, //- air
    electrum, //- lightning
    adamantite, //- earth
    neptunium, //- water
    vulcanium, //- fire
    //Celestial (T6, Hidden)
    nullSteel, //- anti magic
    quickSilver, //- anti monster II
    ichorSteel, //- anti god/boss/elite
    worldTree, //- self repairing
    celestialQuartz, //- 2x all affinities
    soulSteel //- 2x soul drop chance, 2x soul drop rarity
}

public enum MaterialAttribute
{
    none,
    light, //faster speed
    heavy, //slower speed, more dmg
    jagged, //can cause bleeding

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
    tool,
    weaponPart,
        blade,
        handle,
        core,
        crystal,
        harness,
        focus,

    material,
        ore,
        ingot,
        wood,
        stone,
}

public static class ElementalFunctions
{
    private static readonly float matchupModifier = 0.25f;

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
    public struct MaterialData
    {
        public PartMaterial material;
        public int materialLevel;
        public int baseDamage;
        public MaterialAttribute attribute;
        public int attributeLevel;

        public MaterialData(PartMaterial material,  int materialLevel, int baseDamage, MaterialAttribute attribute = MaterialAttribute.none, int attributeLevel = 0)
        {
            this.material = material;
            this.materialLevel = materialLevel;
            this.baseDamage = baseDamage;
            this.attribute = attribute;
            this.attributeLevel = attributeLevel;
        }
    }
    private static List<MaterialData> materialData = new List<MaterialData>()
    {
        new MaterialData(PartMaterial.wood,         1,  5),
        new MaterialData(PartMaterial.stone,        1,  6,  MaterialAttribute.jagged,    1),
        new MaterialData(PartMaterial.aluminium,    1,  6,  MaterialAttribute.light,    2),
        new MaterialData(PartMaterial.copper,       1,  7),

        new MaterialData(PartMaterial.silverwood,   2,  6,  MaterialAttribute.manaEff,  1),
        new MaterialData(PartMaterial.iron,         2,  10),
        new MaterialData(PartMaterial.bronze,       2,  8,  MaterialAttribute.light,    1),
        new MaterialData(PartMaterial.osmium,       2,  14,  MaterialAttribute.heavy,    1),


        new MaterialData(PartMaterial.obsidian,     3,  16,  MaterialAttribute.jagged,    1),
    };

    public static List<MaterialData> GetMaterialsOfLevel(int level)
    {
        if (!materialData.Exists(x => x.materialLevel == level))
            throw new Exception("Material Level: \'" + level + "\' doesnt exist");

        List<MaterialData> materials = new List<MaterialData>();
        foreach (MaterialData mat in materialData)
            if (mat.materialLevel == level)
                materials.Add(mat);

        return materials;
    }
    public static MaterialData GetMaterialData(PartMaterial material)
    {
        return materialData.Find(x => x.material == material);
    }

    public static string GetMaterialName(PartMaterial material)
    {
        switch (material)
        {
            case PartMaterial.wood:
            case PartMaterial.stone:
            case PartMaterial.aluminium:
            case PartMaterial.copper:
            case PartMaterial.silverwood:
            case PartMaterial.iron:
            case PartMaterial.bronze:
            case PartMaterial.osmium:
            case PartMaterial.obsidian:
            case PartMaterial.platinum:
            case PartMaterial.mythril:
            case PartMaterial.electrum:
            case PartMaterial.adamantite:
            case PartMaterial.neptunium:
            case PartMaterial.vulcanium:
            case PartMaterial.silver:
            case PartMaterial.steel:
            case PartMaterial.damascus:
            case PartMaterial.cobalt:
                return UsefulFunctions.Capitalize(material.ToString());
            case PartMaterial.manaSteel:
                return "Mana Steel";
            case PartMaterial.manaStone:
                return "Mana Stone";
            case PartMaterial.shadowSteel:
                return "Shadow Steel";
            case PartMaterial.nullSteel:
                return "Null Steel";
            case PartMaterial.quickSilver:
                return "Quicksilver";
            case PartMaterial.ichorSteel:
                return "Ichor";
            case PartMaterial.worldTree:
                return "Living Wood";
            case PartMaterial.celestialQuartz:
                return "Celestial Quartz";
            case PartMaterial.soulSteel:
                return "Soul Steel";
            default:
            case PartMaterial.none:
                return "";
        }
    }

    public static bool MaterialUsesSuffix(PartMaterial material)
    {
        switch (material)
        {
            case PartMaterial.none:
            case PartMaterial.wood:
            case PartMaterial.stone:
            case PartMaterial.silverwood:
            case PartMaterial.obsidian:
            case PartMaterial.worldTree:
            case PartMaterial.celestialQuartz:
                return false;
        }
        return true;
    }

    

    public static PartMaterial GetMaterialFromIndex(ItemType materialType, int index)
    {
        switch (materialType)
        {
            case ItemType.ore:
            case ItemType.ingot:
                switch (index)
                {
                    case 0:
                        return PartMaterial.aluminium;
                    case 1:
                        return PartMaterial.copper;
                    case 2:
                    default:
                        return PartMaterial.iron;
                    case 3:
                        return PartMaterial.bronze;
                }
            case ItemType.wood:
                switch (index)
                {
                    case 0:
                    default:
                        return PartMaterial.wood;
                    case 1:
                        return PartMaterial.silverwood;
                    case 2:
                        return PartMaterial.worldTree;

                }
            case ItemType.stone:
                switch (index)
                {
                    case 0:
                    default:
                        return PartMaterial.steel;
                    case 1:
                        return PartMaterial.obsidian;
                    case 2:
                        return PartMaterial.celestialQuartz;

                }
            default:
                throw new MissingComponentException("Not a valid material");
        }
    }
    public static MaterialAttribute GetMaterialAttribute(PartMaterial mat)
    {
        switch (mat)
        {
            default:
            case PartMaterial.wood:
            case PartMaterial.stone:
            case PartMaterial.copper:
            case PartMaterial.bronze:
            case PartMaterial.iron:
            case PartMaterial.osmium:
            case PartMaterial.obsidian:
            case PartMaterial.steel:
            case PartMaterial.damascus:
            case PartMaterial.platinum:
                return MaterialAttribute.none;
            case PartMaterial.silverwood:
            case PartMaterial.manaStone:
            case PartMaterial.manaSteel:
                return MaterialAttribute.manaEff;
            case PartMaterial.silver:
                return MaterialAttribute.antiMonster;
            case PartMaterial.cobalt:
                return MaterialAttribute.manaDmg;
            case PartMaterial.shadowSteel:
                return MaterialAttribute.manaResist;
            case PartMaterial.mythril:
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

    public static ResourceType GetResourceType(PartMaterial material, bool isOre = false)
    {
        switch (material)
        {
            case PartMaterial.wood:
            case PartMaterial.silverwood:
                return ResourceType.wood;
            case PartMaterial.stone:
            case PartMaterial.obsidian:
            case PartMaterial.manaStone:
                return ResourceType.stone;
            default:
            case PartMaterial.aluminium:
            case PartMaterial.copper:
            case PartMaterial.iron:
            case PartMaterial.bronze:
            case PartMaterial.osmium:
            case PartMaterial.silver:
            case PartMaterial.steel:
            case PartMaterial.damascus:
            case PartMaterial.cobalt:
            case PartMaterial.manaSteel:
            case PartMaterial.shadowSteel:
            case PartMaterial.platinum:
                if (isOre)
                return ResourceType.ore;
                else
                return ResourceType.ingot;
        }
    }

    public static Material GetMaterialShader(PartMaterial material)
    {
        string path = "Materials/" + material.ToString().ToLower();
        Material mat = Resources.Load<Material>(path);
        if (mat == null)
            throw new Exception("No material found at: " + path);

        return mat;
    }
}

public static class WeaponAtlas
{
    public struct WeaponData
    {
        public WeaponType type;
        public int dmg;
        public WeaponData(WeaponType type, int dmg)
        {
            this.type = type;
            this.dmg = dmg;
        }
    }

    public static List<WeaponData> data = new List<WeaponData>()
    {
        new WeaponData(WeaponType.none, 8),
        new WeaponData(WeaponType.onehanded, 12),
        new WeaponData(WeaponType.katana, 10),
        new WeaponData(WeaponType.rapier, 12),
    };

    public static WeaponData GetData(WeaponType type)
    {
        return data.Find(x => x.type == type);
    }
    public static string GetName(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.onehanded:
                return "Single Sword";
            case WeaponType.twohanded:
                return "Long Sword";
            case WeaponType.katana:
                return "Katana";
            case WeaponType.rapier:
                return "Rapier";
            case WeaponType.none:
            default:
                return "¯\\_(ツ)_/¯";
        }
    }

    public static int CalulateDamage(WeaponType type, PartMaterial material)
    {
        return Mathf.RoundToInt((float)(PartMaterialAtlas.GetMaterialData(material).baseDamage + GetData(type).dmg)/2);
    }
}
