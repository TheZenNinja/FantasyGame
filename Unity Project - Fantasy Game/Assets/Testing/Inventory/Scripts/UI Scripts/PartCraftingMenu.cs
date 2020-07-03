using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class PartCraftingMenu : MonoBehaviour
{
    [SerializeField] bool open;
    public GameObject bg;
    public Slider slider;

    public Vector3 threshholds = new Vector3(1, 0.5f, 0.2f);
    public RectTransform goodUI, perfectUI;

    public ItemSlot inputSlot, outputSlot;
    public ItemSlot reforgeSlot;

    public TextMeshProUGUI outputTxt;

    public TMP_Dropdown partDropdown;
    public TMP_Dropdown subtypeDropdown;


    public bool validRecipe;
    public bool canCraft;
    public bool isCrafting;

    public bool isReforge;

    public AudioSource source;
    public float speed = 2;

    MaterialItem inputItem;
    PartItem reforgeItem;

    private PartPrefab partBeingForged;
    private PartMaterial partMaterial;

    private void Start()
    {
        goodUI.sizeDelta = new Vector2(threshholds.y * 500, goodUI.sizeDelta.y);
        perfectUI.sizeDelta = new Vector2(threshholds.z * 500, perfectUI.sizeDelta.y);

        inputSlot.OnDataChange += ValidateInput;
        reforgeSlot.OnDataChange += ValidateInput;
    }

    public void Update()
    {
        if (!open)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            ButtonPress();
        if (isCrafting)
            slider.value = Mathf.Sin(Time.time * speed);
    }
    public void Open(AudioSource source)
    {
        this.source = source;
        bg.SetActive(true);
        open = true;
        UpdateDropDown();
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
        open = false;
    }

    public void UpdateDropDown()
    {
        if (isCrafting)
            return;

        partDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (PartPrefab part in PartAtlas.GetAllBaseParts())
            options.Add(part.name);
        partDropdown.AddOptions(options);

        UpdateSubDropdown();
    }
    public void UpdateSubDropdown()
    {
        PartType type = (PartType)partDropdown.value;

        subtypeDropdown.ClearOptions();
        List<string> options = new List<string>();

        var parts = PartAtlas.GetPartsOfType(type);
        for (int i = 0; i < parts.Count; i++)
        {
            if (i == 0)
                options.Add("Basic");
            else
                options.Add(parts[i].name);
        }
        subtypeDropdown.AddOptions(options);
        UpdateOutputData();
    }

    public void ButtonPress()
    {
        if (isCrafting)
        {
            if (outputSlot.hasItem())
            {
                outputTxt.text = "Remove Item From Output";
                return;
            }

                if (Mathf.Abs(slider.value) <= threshholds.z)
                    Forge(2);
                else if (Mathf.Abs(slider.value) <= threshholds.y)
                    Forge(1);
                else
                    Forge(0);


            isReforge = false;
            isCrafting = false;
        }
        else if (validRecipe && canCraft)
        {
            ConsumeItems();
            isCrafting = true;
            slider.value = Random.Range(-1f, 1f);
        }
    }
    public void ValidateInput()
    {
        if (isCrafting)
            return;
        validRecipe = false;
        isReforge = false;

        if (reforgeSlot.hasItem() && reforgeSlot.GetItem().GetType() == typeof(PartItem))
            reforgeItem = (PartItem)reforgeSlot.GetItem();
        else
            reforgeItem = null;

        if (inputSlot.hasItem() && inputSlot.GetItem().GetType() == typeof(MaterialItem)
            && ((MaterialItem)inputSlot.GetItem()).resourceType != ResourceType.ore)
            inputItem = (MaterialItem)inputSlot.GetItem();
        else
            inputItem = null;

        if (inputItem)
            partMaterial = inputItem.material;

        if (reforgeItem)
        {
            if (reforgeItem.part.material == partMaterial)
            {
                isReforge = true;
                partBeingForged = PartAtlas.GetPart(reforgeItem.part.type, reforgeItem.part.subType);
                if (partBeingForged != null)
                    validRecipe = true;
            }
        }
        else
        {
            partBeingForged = PartAtlas.GetPart(partDropdown.value, subtypeDropdown.value);
            if (partBeingForged != null && inputSlot.GetCount() >= partBeingForged.craftingCost)
                validRecipe = true;
        }

        UpdateOutputData();
    }
    public void UpdateOutputData()
    {
        outputTxt.text = "Null Error";
        partBeingForged = PartAtlas.GetPart(partDropdown.value, subtypeDropdown.value);

        if (partBeingForged == null)
            return;

        //validRecipe = true;
        outputTxt.text = "Part: " + partBeingForged.name + "\nCost: " + partBeingForged.craftingCost;

        if (isReforge)
        {
            outputTxt.text = "Reforging: " + reforgeItem.part.name;
            canCraft = true;
        }
        else if (inputItem)
        {
            outputTxt.text += "\nMaterial: " + ((MaterialItem)inputSlot.GetItem()).material.ToString();
            canCraft = true;
        }
    }
    public void ConsumeItems()
    {
            if (isReforge)
            {
                inputSlot--;
                reforgeSlot.ClearItem();
            }
            else
        inputSlot -= partBeingForged.craftingCost;
    }
    public void Forge(int quality)
    {
        outputSlot.SetItem(new PartItem(new Part(partBeingForged, partMaterial, quality)));
        source.Play();
    }
}