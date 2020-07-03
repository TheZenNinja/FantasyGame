using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmeltingMenu : MonoBehaviour
{
    public GameObject bg;
    public bool isOpen;


    [Header("Ore")]

    bool trySmelt;
    bool canSmelt = false;
    bool oreSmelting;

    public float smeltTime = 1;
    private float currentSmeltTime;

    public ItemSlot smeltInput;
    public ItemSlot smeltOutput;

    public Image fireSprite;

    [Header("Alloy")]
    public ItemSlot alloyInput1;
    public ItemSlot alloyInput2;
    public ItemSlot alloyOutput;
    [SerializeField] bool validAlloy;
    [SerializeField] AlloyRecpie currentRecipe;
    public TextMeshProUGUI outputTxt;


    void Start()
    {
        Close();

            alloyInput1.OnDataChange += UpdateUI;
            alloyInput2.OnDataChange += UpdateUI;
    }
    private void Update()
    {
        Smelting();
    }

    public void Open()
    {
        bg.SetActive(true);
        fireSprite.fillAmount = 0;
        UpdateUI();
        isOpen = true;
    }
    public void Close()
    {
        ItemSlot[] slots = new ItemSlot[] { alloyInput1, alloyInput2, alloyOutput };
        foreach (ItemSlot slot in slots)
            if (slot.hasItem())
            {
                Vector3 rand = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                Instantiate(WorldItem.CreateWorldItem(slot.GetItem(), slot.GetCount()), FindObjectOfType<PlayerMove>().transform.position + rand, Quaternion.identity);
                slot.ClearItem();
            }

        bg.SetActive(false);
        isOpen = false;
    }
    #region Smelting

    public void ToggleSmelting()
    {
        trySmelt = !trySmelt;
    }

    private void Smelting()
    {
        if (trySmelt)
        {
            if (!oreSmelting)
            {
                canSmelt = false;

                if (smeltInput.hasItem())
                    if (smeltInput.GetItem().GetType() == typeof(MaterialItem))
                        if (smeltOutput.hasItem())
                        {
                            if ((smeltInput.GetItem() as MaterialItem).material == (smeltOutput.GetItem() as MaterialItem).material)
                                canSmelt = true;
                        }
                        else
                            canSmelt = true;

            }

            if (canSmelt)
            {
                if (oreSmelting)
                {
                    currentSmeltTime += 1f / smeltTime * Time.deltaTime;
                    fireSprite.fillAmount = currentSmeltTime / smeltTime;

                    if (currentSmeltTime >= smeltTime)
                        SmeltOre();
                }
                else
                {
                    currentSmeltTime = 0;
                    oreSmelting = true;
                    fireSprite.fillAmount = 0;
                }
            }
            else
            {
                currentSmeltTime = 0;
                oreSmelting = false;
                fireSprite.fillAmount = 0;
                trySmelt = false;

            }
        }
    }
    private void SmeltOre()
    {
        currentSmeltTime = 0;
        oreSmelting = false;
        MaterialItem mi = smeltInput.GetItem() as MaterialItem;
        MaterialItem output = new MaterialItem(ResourceType.ingot, mi.material);
        smeltInput--;

        if (!smeltOutput.hasItem())
        {
            smeltOutput.SetItem(output);
        }
        else if ((smeltOutput.GetItem() as MaterialItem).SameItem(output))
        {
            smeltOutput++;
        }
    }
    #endregion
    public void UpdateUI()
    {
        CheckRecipe();
        outputTxt.text = "";

        if (currentRecipe != null)
        {
            outputTxt.text += "Output:\n" + currentRecipe.output.ToString();
        }
    }

    public void ForgeAlloy()
    {
        if (currentRecipe == null)
            return;
        if (alloyOutput.hasItem() && (alloyOutput.GetItem() as MaterialItem).material != currentRecipe.output)
            return;
        if (!alloyInput1.hasItem() || !alloyInput2.hasItem())
                return;
        

        if (alloyOutput.hasItem())
        {
            alloyOutput += 2;
        }
        else
        {
            PartMaterial material = currentRecipe.output;

            MaterialItem outputItem = new MaterialItem(ResourceType.ingot, material);

            alloyOutput.SetItem(outputItem, 2);
        }
        alloyInput1--;
        alloyInput2--;
    }

    private void CheckRecipe()
    {
        currentRecipe = null;
        validAlloy = false;
        if (!alloyInput1.hasItem() || !alloyInput2.hasItem())
            return;

        for (int i = 0; i < alloyRecpies.Count; i++)
        {
            MaterialItem mi1 = alloyInput1.GetItem() as MaterialItem;
            MaterialItem mi2 = alloyInput2.GetItem() as MaterialItem;

            List<PartMaterial> inputMats = new List<PartMaterial>() { mi1.material, mi2.material };

            if (alloyRecpies[i].isRecipe(inputMats))
            {
                currentRecipe = alloyRecpies[i];
                validAlloy = true;
                return;
            }
        }
    }

    [System.Serializable]
    private class AlloyRecpie
    {
        public PartMaterial[] input;
        public PartMaterial output;

        public AlloyRecpie(PartMaterial[] input, PartMaterial output)
        {
            this.input = input;
            this.output = output;
        }

        public bool isRecipe(List<PartMaterial> inputs)
        {
            for (int i = 0; i < input.Length; i++)
                if (!inputs.Contains(input[i]))
                    return false;

            return true;
        }
    }
    private List<AlloyRecpie> alloyRecpies = new List<AlloyRecpie>()
    { //Bronze
    new AlloyRecpie(
        /*input*/new PartMaterial[2] { PartMaterial.copper, PartMaterial.aluminium }, /*Output*/PartMaterial.bronze)
    };

    /*private struct AlloyRecipeComponent
    {
        public PartMaterial material;
        public int count;

        public AlloyRecipeComponent(PartMaterial material, int count)
        {
            this.material = material;
            this.count = count;
        }
    }

    private class AlloyRecpie
    {
        public AlloyRecipeComponent[] input;
        public AlloyRecipeComponent[] output;

        public AlloyRecpie(AlloyRecipeComponent[] input, AlloyRecipeComponent[] output)
        {
            this.input = input;
            this.output = output;
        }

        public bool isRecipe(List<AlloyRecipeComponent> inputs)
        {
            for (int i = 0; i < input.Length; i++)
                if (!inputs.Contains(input[i]))
                    return false;

            return true;
        }
    }

    private List<AlloyRecpie> alloyRecpies = new List<AlloyRecpie>()
    { //Bronze
    new AlloyRecpie(
        //input
        new AlloyRecipeComponent[2] { new AlloyRecipeComponent(PartMaterial.copper, 3), new AlloyRecipeComponent(PartMaterial.aluminium, 1) },
        //output
        new AlloyRecipeComponent[1] { new AlloyRecipeComponent(PartMaterial.bronze, 4) })
    };*/
}
