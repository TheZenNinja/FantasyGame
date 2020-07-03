using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AssemblyMenuType
{
    none,   //list subtabs
    tool,   //none
    sword,  //standard, greatsword, katana, rapier
    //bow,  //basic, longbow, recurvebow, crossbow
    //gun,  
}
public class AssemblyMenu : MonoBehaviour
{
    private static List<AssemblySubMenu> submenus = new List<AssemblySubMenu>()
    {
        new AssemblySubMenu(AssemblyMenuType.tool, new ModularRecipe[]
        {
            new ModularRecipe( new PartType[] {PartType.pickHead, PartType.rod}, EquipmentType.tool),
            new ModularRecipe( new PartType[] {PartType.axeHead, PartType.rod}, EquipmentType.tool),
            new ModularRecipe( new PartType[] {PartType.hammerHead, PartType.rod}, EquipmentType.tool)
        }),
        new AssemblySubMenu(AssemblyMenuType.sword, new ModularRecipe[]
        {
            //One handed
            new ModularRecipe( new PartType[] {PartType.mediumBlade, PartType.crossguard, PartType.handle}, EquipmentType.sword, WeaponType.onehanded), //basic sword
            //Katana
            new ModularRecipe( new PartType[] {PartType.curvedBlade, PartType.crossguard, PartType.handle/*, PartType.sheath*/}, EquipmentType.sword, WeaponType.katana), //Katana
            //Rapier
            new ModularRecipe( new PartType[] {PartType.smallBlade, PartType.handle}, EquipmentType.sword, WeaponType.rapier), //rapier

            //new ModularRecipe( new PartType[] {PartType.smallBlade, PartType.handle}, EquipmentType.sword, WeaponType.onehanded), //dagger
        })
    };

    public GameObject bg;

    public bool canCraft;
    public bool canDisassemble;
    public Button craftButton;
    public Button disassembleButton;

    ModularRecipe currentRecipe;
    public TextMeshProUGUI recipeOutputTxt;
    public ItemSlot[] inputs;
    public ItemSlot output;

    public int currentTab;
    public Image[] tabSprites;
    AudioSource source;
    private void Start()
    {
        foreach (ItemSlot slot in inputs)
            slot.OnDataChange += CheckCrafting;

        output.OnDataChange += CheckCrafting;

        SwitchTab(0);
    }

    public void Open(AudioSource source, AssemblyMenuType assemblyType)
    {
        this.source = source;
        bg.SetActive(true);
    }
    public void Close()
    {
        foreach (ItemSlot slot in GetComponentsInChildren<ItemSlot>())
            if (slot.hasItem())
            {
                Vector3 rand = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

                Instantiate(WorldItem.CreateWorldItem(slot.GetItem(), slot.GetCount()), FindObjectOfType<PlayerMove>().transform.position + rand, Quaternion.identity);
                slot.ClearItem();
            }
        bg.SetActive(false);
    }

    public void SwitchTab(int index)
    {
        currentTab = index;

        for (int i = 0; i < tabSprites.Length; i++)
            tabSprites[i].color = i == index ? Color.white : Color.gray;

        CheckCrafting();
    }
    public void UpdateUI()
    {
        craftButton.interactable = canCraft;
        disassembleButton.interactable = canDisassemble;

        if (currentRecipe != null)
        {
            switch (currentRecipe.outputEquipType)
            {
                default:
                case EquipmentType.none:
                    recipeOutputTxt.text = "Error";
                    break;
                case EquipmentType.tool:
                    recipeOutputTxt.text = currentRecipe.outputEquipType.ToString();
                    break;
                case EquipmentType.sword:
                case EquipmentType.bow:
                case EquipmentType.gun:
                    recipeOutputTxt.text = WeaponAtlas.GetName(currentRecipe.outputWeapType);
                    break;
            }
        }
        else
            recipeOutputTxt.text = "Not A Recipe";
    } 


    public void CheckCrafting()
    {
        canDisassemble = false;
        canCraft = false;
        currentRecipe = null;

        bool inputsEmpty = true;
        foreach (ItemSlot slot in inputs)
            if (slot.hasItem())
            {
                inputsEmpty = false;
                break;
            }

        if (output.hasItem() && inputsEmpty)
        {
            if (output.GetItem().GetType() == typeof(ModularItem))
            {
                canDisassemble = true;
                recipeOutputTxt.text = "Disassemble";
            }
        }
        else if (!inputsEmpty)
        {
            List<Part> parts = new List<Part>();
            foreach (ItemSlot slot in inputs)
                if (slot.hasItem())
                {
                    if (slot.GetItem().GetType() == typeof(PartItem))
                        parts.Add(((PartItem)slot.GetItem()).part);
                    else
                        return;
                }
            if (parts.Count > 1)
                foreach (ModularRecipe recipe in submenus[currentTab].recipes)
                {
                    if (recipe.Matches(parts))
                    {
                        currentRecipe = recipe;
                        canCraft = true;
                        break;
                    }
                }
        }

        UpdateUI();
    }

    public void Craft()
    {
        if (!canCraft)
            return;

        List<Part> parts = new List<Part>();
        for (int i = 0; i < inputs.Length; i++)
            if (inputs[i].hasItem())
            {
                PartItem item = inputs[i].GetItem() as PartItem;
                parts.Add(item.part);
            }

        ModularItem newItem = new ModularItem(parts, currentRecipe.outputWeapType);

        foreach (ItemSlot item in inputs)
            item.ClearItem();

        output.SetItem(newItem);
    }
    public void DisassembleTool()
    {
        if (!canDisassemble)
            return;

        ModularItem item = output.GetItem() as ModularItem;

        List<PartItem> items = new List<PartItem>();
        for (int i = 0; i < item.parts.Count; i++)
            inputs[i].SetItem(new PartItem(item.parts[i]));

        output.ClearItem();
    }

    private class AssemblySubMenu
    {
        public AssemblyMenuType menuType;

        public List<ModularRecipe> recipes;

        public AssemblySubMenu(AssemblyMenuType menuType, ModularRecipe[] recipes)
        {
            this.menuType = menuType;
            this.recipes = new List<ModularRecipe>(recipes);
        }
    }

    private class ModularRecipe
    {
        public List<PartType> requiredParts;
        public EquipmentType outputEquipType;
        public WeaponType outputWeapType;
        public ModularRecipe(List<PartType> requiredParts, EquipmentType outputEquipType, WeaponType outputWeapType = WeaponType.none)
        {
            this.requiredParts = requiredParts;
            this.outputEquipType = outputEquipType;
            this.outputWeapType = outputWeapType;
        }
        public ModularRecipe(PartType[] requiredParts, EquipmentType outputEquipType, WeaponType outputWeapType = WeaponType.none)
        {
            this.requiredParts = new List<PartType>(requiredParts);
            this.outputEquipType = outputEquipType;
            this.outputWeapType = outputWeapType;
        }

        public bool Matches(List<Part> parts)
        {
            int matches = 0;
            foreach (Part part in parts)
                if (requiredParts.Contains(part.type))
                    matches++;

            return matches >= requiredParts.Count;
        }
    }
}
