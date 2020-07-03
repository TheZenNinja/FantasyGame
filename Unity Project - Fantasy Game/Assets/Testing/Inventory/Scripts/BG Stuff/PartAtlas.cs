using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PartAtlas
{
    public static List<PartPrefab> allParts = new List<PartPrefab>()
    {
        new PartPrefab("Rod", PartType.rod,0,2),
        new PartPrefab("Long Rod", PartType.rod,1,3),
        new PartPrefab("Magnum Rod", PartType.rod,2,69),
        new PartPrefab("Meme Rod", PartType.rod,3,420),

        //new PartPrefab("Plate", PartType.plate,0,8),

        new PartPrefab("Axe Head", PartType.axeHead, 0,6),
        new PartPrefab("Pickaxe Head", PartType.pickHead, 0,6),
        new PartPrefab("Hammer Head", PartType.hammerHead, 0,8),

        new PartPrefab("Handle", PartType.handle, 0,3),
        new PartPrefab("Light Handle", PartType.handle, 1,2),
        new PartPrefab("Curved Handle", PartType.handle, 2,3),
        new PartPrefab("Tsuka", PartType.handle, 3,3),

        new PartPrefab("Guard", PartType.crossguard, 0,4),
        new PartPrefab("Small Guard", PartType.crossguard, 2,2),
        new PartPrefab("Tsuba", PartType.crossguard, 1,2), //katana

        //new PartPrefab("Hand Guard", PartType.crossguard, 3,4), //rapier
        //new PartPrefab("Forward Guard", PartType.crossguard, 4,6), //broadsword

        new PartPrefab("Small Blade", PartType.smallBlade, 0,4), //sword: rapier
        new PartPrefab("Sword Blade", PartType.mediumBlade, 0,8), //sword: basic
        new PartPrefab("Curved Blade", PartType.curvedBlade, 0,8), //sword: Katana
        //new PartPrefab("Large Blade", PartType.largeBlade, 0,12),

        //new PartPrefab("Sheath", PartType.sheath, 0,8),
    };


    public static string GetToolName(PartType type)
    {
        switch (type)
        {
            case PartType.axeHead:
                return "Axe";
            case PartType.pickHead:
                return "Pickaxe";
            case PartType.hammerHead:
                return "Hammer";
            default:
                throw new Exception("Not a valid tool");
        }
    }

    

    public static List<PartPrefab> GetAllBaseParts()
    {
        List<PartPrefab> parts = new List<PartPrefab>();
        foreach (PartPrefab part in allParts)
            if (!parts.Exists(x => x.type == part.type) && part.subType == 0)
                parts.Add(part);

        return parts;
    }

    public static List<PartPrefab> GetPartsOfType(PartType type)
    {
        List<PartPrefab> parts = new List<PartPrefab>();
        foreach (PartPrefab part in allParts)
            if (part.type == type)
                parts.Add(part);

        parts.Sort(new PartSubtypeComparison());

        return parts;
    }
    public static bool TypeHasSubtypes(PartType type)
    {
        List<PartPrefab> parts = new List<PartPrefab>();
        foreach (PartPrefab part in allParts)
            if (part.type == type)
                parts.Add(part);


        return parts.Count > 1;
    }
    public static PartPrefab GetPart(int type, int subtype)
    {
        return GetPart((PartType)type, subtype);
    }
    public static PartPrefab GetPart(PartType type, int subtype)
    {
        foreach (PartPrefab part in allParts)
            if (part.type == type && part.subType == subtype)
                return part;

        return null;
    }
    public static string GetPartName(Part p)
    {
        return GetPart(p.type, p.subType).name;
    }
    public static EquipmentType GetEquipmentType(PartType type)
    {
        switch (type)
        {
            default:
            case PartType.rod:
            //case PartType.plate:
            case PartType.handle:
            //case PartType.sheath:
            //case PartType.prism:
                return EquipmentType.none;
            case PartType.axeHead:
            case PartType.pickHead:
            case PartType.hammerHead:
                return EquipmentType.tool;
            case PartType.smallBlade:
            case PartType.mediumBlade:
            case PartType.curvedBlade:
            //case PartType.largeBlade:
                return EquipmentType.sword;
        }
    }
    private class PartSubtypeComparison : IComparer<PartPrefab>
    {
        public int Compare(PartPrefab x, PartPrefab y)
        {
            if (x.subType > y.subType)
                return 1;
            else if (x.subType < y.subType)
                return -1;
            else
                return 0;
        }
    }
    private class PartTypeComparison : IComparer<Part>
    {
        public int Compare(Part x, Part y)
        {
            int a, b;

            switch (x.type)
            {
                case PartType.rod:
                case PartType.handle:
                    a = 0;
                    break;
                case PartType.crossguard:
                    a = 1;
                    break;
                case PartType.axeHead:
                case PartType.pickHead:
                case PartType.hammerHead:
                    a = 2;
                    break;
                case PartType.smallBlade:
                case PartType.mediumBlade:
                case PartType.curvedBlade:
                //case PartType.largeBlade:
                    a = 2;
                    break;
                //case PartType.sheath:
                //    a = 3;
                //    break;
                //case PartType.prism:
                //case PartType.plate:
                default:
                    a = Mathf.RoundToInt(Mathf.Infinity);
                    break;
            }
            switch (y.type)
            {
                case PartType.rod:
                case PartType.handle:
                    b = 0;
                    break;
                case PartType.crossguard:
                    b = 1;
                    break;
                case PartType.axeHead:
                case PartType.pickHead:
                case PartType.hammerHead:
                    b = 2;
                    break;
                case PartType.smallBlade:
                case PartType.mediumBlade:
                case PartType.curvedBlade:
                //case PartType.largeBlade:
                    b = 2;
                    break;
                //case PartType.sheath:
                //    b = 3;
                //    break;
                //case PartType.prism:
                //case PartType.plate:
                default:
                    b = Mathf.RoundToInt(Mathf.Infinity);
                    break;
            }

            if (a > b)
                return 1;
            else if (a < b)
                return -1;
            else
                return 0;
        }
    }
    public static List<Part> SortPartsForConstruction(List<Part> parts)
    {
        parts.Sort(new PartTypeComparison());
        return parts;
    }


    //equipment type, part type, subtype, resource type
    public static GameObject GetPartMesh(EquipmentType equipmentType, PartType partType, int partSubtype, WeaponType weaponType = WeaponType.none)//, ResourceType resourceType)
    {
        bool singleSubtype = !TypeHasSubtypes(partType);
        string path;


        switch (equipmentType)
        {
            default:
            case EquipmentType.none:
            case EquipmentType.tool:
                if (singleSubtype)
                    path = "parts/" + equipmentType + "/" + partType;
                else
                    path = "parts/" + equipmentType + "/" + partType + "/" + partType + partSubtype;
                break;
            case EquipmentType.sword:
                path = "parts/" + equipmentType + "/" + partType + "/" + partType + partSubtype;
                break;
                /*case EquipmentType.bow:
                case EquipmentType.gun:
                    break;*/
        }

        //Debug.Log("Attempting to get part at:\n" + path);
        GameObject g = Resources.Load<GameObject>(path);
        /*if (g == null)
            Debug.LogWarning("No prefab found");
        else
            Debug.Log("Prefab found");*/

        return g;
    }
}
public class PartPrefab
{
    public string name;
    public PartType type;
    public int subType;
    public int craftingCost;
    public WeaponType ofWeaponType;
    public PartPrefab(string name, PartType type, int subType, int craftingCost, WeaponType ofWeaponType = WeaponType.none)
    {
        this.name = name;
        this.type = type;
        this.subType = subType;
        this.craftingCost = craftingCost;
        this.ofWeaponType = ofWeaponType;
    }

    public override string ToString()
    {
        return name;
    }
}
[System.Serializable]
public class Part
{
    public string name;
    public PartType type;
    public int subType;
    public PartMaterial material;
    [Tooltip("0 = ok, 1 = good, 2 = perfect")]
    public int quality = 0;

    public Part(PartPrefab prefab, PartMaterial material, int quality)
    {
        if (prefab == null)
                throw new System.Exception("Part is null");

        if (quality == 2)
            this.name = prefab.name + "+";
        else
            this.name = prefab.name;
        this.type = prefab.type;
        this.subType = prefab.subType;
        this.material = material;
        this.quality = quality;
    }

    public Part(string name, PartType type, int subType, PartMaterial material, int quality)
    {
        this.name = name;
        this.type = type;
        this.subType = subType;
        this.material = material;
        this.quality = quality;
    }
    /*public Part Clone()
    {
        return new Part(name, type, subType, material, quality);
    }*/
    public override string ToString()
    {
        string qual;
        switch (quality)
        {
            default:
                qual = "Decent";
                break; 
            case 1:
                qual = "Excellent";
                break;
            case 2:
                qual = "Perfect";
                break;
        }


        return "Type: " + PartAtlas.GetPartName(this) + "\nMaterial: " + UsefulFunctions.Capitalize(material.ToString()) + "\nForge Level: " + qual;
    }

    
}

public enum PartType
{
    //general
    rod,
    //core,
    //plate,
    //tools
    axeHead,
    pickHead,
    hammerHead,
    //weapons
    handle,

    crossguard,

    smallBlade,
    mediumBlade,
    curvedBlade,
    //largeBlade,

    //other parts
    //sheath, //katana
    //prism
}