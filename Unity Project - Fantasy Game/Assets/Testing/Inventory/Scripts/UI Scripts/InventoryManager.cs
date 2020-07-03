using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private void Awake()
    { instance = this; }
    
    [SerializeField] ItemSlot dragSlot;

    //public PlayerSaveData saveData;
    [Header("Controls")]
    public bool inUI;
    public StorageMenu playerInventory;
    public EquipmentManager equipmentMenu;
    public PartCraftingMenu anvilMenu;
    public AssemblyMenu assemblyMenu;
    public SmeltingMenu smeltingMenu;
    public PlayerLook playerLook;

    [Header("PauseMenu")]
    public GameObject pauseMenuBG;
    public bool pauseMenuOpen;
    PlayerMove move;

    [Header("Item Description")]
    [SerializeField] Transform descPos;
    [SerializeField] GameObject descBG;
    [SerializeField] TextMeshProUGUI itemNameTxt;
    [SerializeField] TextMeshProUGUI itemCountTxt;
    [SerializeField] TextMeshProUGUI itemDescTxt;

    private void Start()
    {
        dragSlot.OnDataChange += () => { dragSlot.gameObject.SetActive(dragSlot.hasItem()); };

        move = FindObjectOfType<PlayerMove>();

        CloseUI();
        pauseMenuBG.SetActive(false);
    }

    private void Update()
    {
        if (inUI)
        {
            if (dragSlot.hasItem())
            {
                if (!dragSlot.gameObject.activeSelf)
                    dragSlot.gameObject.SetActive(true);

                dragSlot.transform.position = Input.mousePosition;
            }

            if (descBG.activeInHierarchy)
            {
                descPos.position = Input.mousePosition;
                if (descPos.position.x > 500)
                    descBG.transform.localPosition = new Vector3(-225, -25, 0);
                else if (descPos.position.x < -500)
                    descBG.transform.localPosition = new Vector3(225, -25, 0);
            }
        }
        else if (dragSlot.hasItem())
        {
            Vector3 rand = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

            Instantiate(WorldItem.CreateWorldItem(dragSlot.GetItem(), dragSlot.GetCount()), FindObjectOfType<PlayerMove>().transform.position + rand, Quaternion.identity);
            dragSlot.ClearItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CloseUI();
            OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inUI)
                CloseUI();
            else
            {
                DisableDesc();
                pauseMenuOpen = !pauseMenuOpen;
                inUI = pauseMenuOpen;
                pauseMenuBG.SetActive(pauseMenuOpen);
                playerLook.MakeCursorActive(pauseMenuOpen);
            }
        }
    }

    #region Menu Stuff
    public void OpenInventory()
    {
        playerInventory.Open();
        equipmentMenu.Open();
        playerLook.MakeCursorActive(true);
        inUI = true;
        move.canMove = false;
    }
    public void OpenCrafting(AudioSource source,AssemblyMenuType assemblyType)
    {
        playerInventory.Open();
        assemblyMenu.Open(source, assemblyType);
        playerLook.MakeCursorActive(true);
        inUI = true;
        move.canMove = false;
    }
    public void OpenAnvil(AudioSource source)
    {
        playerInventory.Open();
        anvilMenu.Open(source);
        playerLook.MakeCursorActive(true);
        inUI = true;
        move.canMove = false;
    }
    public void OpenSmelting()
    {
        playerInventory.Open();
        smeltingMenu.Open();
        playerLook.MakeCursorActive(true);
        inUI = true;
        move.canMove = false;
    }
    //change later if performance is bad
    public void CloseUI()
    {
        playerInventory.Close();
        equipmentMenu.Close();
        assemblyMenu.Close();
        anvilMenu.Close();
        smeltingMenu.Close();
        playerLook.MakeCursorActive(false);
        inUI = false;
        DisableDesc();
        move.canMove = true;
        //Save();
    }
    #endregion

    #region Item Swapping
    public void OnClick(ItemSlot slot, bool rightClick)
    {
        
        if (dragSlot.hasItem())
        {
            if (!rightClick || (!slot.hasItem() && dragSlot.GetCount() == 1))
            {
                if (slot.hasItem() && slot.GetItem().stackable && slot.GetItem().SameItem(dragSlot.GetItem()))
                {
                    slot += dragSlot;
                    dragSlot.ClearItem();
                }
                else
                {
                    Item ci = dragSlot.GetItem();
                    int cc = dragSlot.GetCount();

                    dragSlot.SetItem(slot.GetItem(), slot.GetCount());

                    slot.SetItem(ci, cc);
                }
            }
            else if (rightClick)
            {
                if (!slot.hasItem())
                {
                    slot.SetItem(dragSlot.GetItem());
                    dragSlot--;
                    if (dragSlot.GetCount() <= 0)
                        dragSlot.ClearItem();
                }
                else if (slot.GetItem().stackable && slot.GetItem().SameItem(dragSlot.GetItem()))
                {
                    slot++;
                    dragSlot--;
                    if (dragSlot.GetCount() <= 0)
                        dragSlot.ClearItem();
                }
            }
        }
        else //If an item isnt being dragged
        {
            if (!slot.hasItem())
                return;

            if (!rightClick || slot.GetCount() == 1)
            {
                dragSlot.SetItem(slot.GetItem(), slot.GetCount());
                slot.ClearItem();
            }
            else
            {
                float count = slot.GetCount();

                dragSlot.SetItem(slot.GetItem().Duplicate(),Mathf.CeilToInt(count / 2));

                slot.SetCount(Mathf.FloorToInt(count / 2));
            }
        }
    }
    #endregion
    
    #region Saving
    public void SaveData()
    {
        PlayerSaveData save = new PlayerSaveData();

        save.SetItems(playerInventory.items, playerInventory.itemCount);
        save.SetEquipment(equipmentMenu.GetHotbar());

        SaveItemToJson.SaveData(save);
    }

    public void LoadData()
    {
        PlayerSaveData save = SaveItemToJson.LoadData();
        playerInventory.SetItems(save.GetItems());
        equipmentMenu.SetItems(save.GetEquipment());
    }
    #endregion

    #region Item Description
    public void DisableDesc()
    {
        descBG.SetActive(false);
    }

    public void SetDesc(ItemSlot slot)
    {
        if (slot.hasItem())
        {
            descBG.SetActive(true);

            itemNameTxt.text = slot.GetItem().GetName();

            itemDescTxt.text = "";

            if (slot.GetItem().stackable)
                itemCountTxt.text = "Count: " + slot.GetCount();
            else
                itemCountTxt.text = "";

            itemDescTxt.text += "\n" + slot.GetItem().description;
        }
        else
            descBG.SetActive(false);
    }
    #endregion

    private void OnDestroy()
    {
        //SaveData();
    }

    public void ExitGame()
    {
        FindObjectOfType<SceneLoader>().LoadScene(0);
    }
}
