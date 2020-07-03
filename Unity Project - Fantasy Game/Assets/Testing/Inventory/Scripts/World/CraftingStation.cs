using UnityEngine;
using System.Collections;

public class CraftingStation : MonoBehaviour,IInteractable
{
    public AudioSource source;
    private enum StationType
    {
        crafting, smelting, forging
    }
    [SerializeField] StationType stationType;
    [SerializeField] AssemblyMenuType assemblyType;
    public void Interact()
    {
        switch (stationType)
        {
            case StationType.crafting:
                InventoryManager.instance.OpenCrafting(source, assemblyType);
                break;
            case StationType.smelting:
                InventoryManager.instance.OpenSmelting();
                break;
            case StationType.forging:
                InventoryManager.instance.OpenAnvil(source);
                break;
        }
    }
}
