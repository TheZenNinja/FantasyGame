using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ResourceType resourceType;
    public PartMaterial material;

    [Space]
    public Item item;



    public int count;
    public void Pickup()
    {
        StorageMenu inv = InventoryManager.instance.playerInventory;
        if (item)
        {
            if (inv.AddItem(item, count))
                Destroy(gameObject);
        }
        else
        {
            Item newItem = new MaterialItem(resourceType, material);

            if (inv.AddItem(newItem, count))
                    Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMove>())
        {
            Pickup();
        }
    }
    public void SetResourceData(ResourceType resourceType, PartMaterial material, int count)
    {
        this.resourceType = resourceType;
        this.material = material;
        this.count = count;
    }
    public static GameObject CreateWorldItem(Item item, int count)
    {
        WorldItem worldItem = Resources.Load<GameObject>("World Item").GetComponent<WorldItem>();

        worldItem.item = item;
        worldItem.count = count;

        return worldItem.gameObject;
    }
}
