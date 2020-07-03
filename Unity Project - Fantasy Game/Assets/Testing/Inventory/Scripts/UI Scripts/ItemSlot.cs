using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Item item;
    [SerializeField]
    protected int count;
    [SerializeField] protected Image rarityBG;
    [SerializeField] protected Image sprite;
    [SerializeField] protected Sprite bgSprite;
    [SerializeField] protected TextMeshProUGUI text;
    public SlotType type;

    public event Action OnDataChange;

    public event Action OnCountChange;
    public event Action OnSetItem;
    public event Action OnTakeItem;
    public event Action OnRemoveItem;

    private void Awake()
    {
        OnDataChange += UpdateUI;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (hasItem())
        {
            sprite.color = new Color(0.2f, 0.2f, 0.2f, 1);

            if (count > 1)
            {
                text.enabled = true;
                text.text = count.ToString();
                sprite.rectTransform.localScale = Vector3.one * 0.75f;
            }
            else
            {
                text.enabled = false;
                sprite.rectTransform.localScale = Vector3.one;
            }

            sprite.enabled = true;
            sprite.sprite = item.sprite;
            rarityBG.color = Item.GetColor(item.rarity);
        }
        else
        {
            text.enabled = false;
            if (bgSprite)
            {
                sprite.enabled = true;
                sprite.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
                sprite.sprite = bgSprite;
            }
            else
            {
                sprite.enabled = false;
            }
            rarityBG.color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    public bool hasItem()
    {
        return item != null;
    }

    private void OnValidate()
    {
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.instance.OnClick(this, eventData.button == PointerEventData.InputButton.Right);
    }
    public void ClearItem()
    {
        count = 0;
        item = null;
        OnDataChange?.Invoke();
        OnRemoveItem?.Invoke();
    }
    public void SetItem(Item i, int c = 1)
    {
        count = c;
        item = i;
        OnDataChange?.Invoke();
        OnSetItem?.Invoke();
    }
    public void SetItem(InventorySlot data) => SetItem(data.item, data.count);

    public InventorySlot TakeItem()
    {
        InventorySlot inv = new InventorySlot(item, count);
        ClearItem();

        OnDataChange?.Invoke();
        OnTakeItem?.Invoke();

        return inv;
    }
    public Item GetItem()
    {
        return item;
    }
    public int GetCount()
    {
        return count;
    }
    public void SetCount(int count)
    {
        this.count = count;
        OnDataChange?.Invoke();
        OnCountChange?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.DisableDesc();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.instance.SetDesc(this);
    }

    public static ItemSlot operator +(ItemSlot a, ItemSlot b)
    {
        if (a.item.SameItem(b.item))
        {
            a.count += b.count;
            a.OnDataChange?.Invoke();
        a.OnCountChange?.Invoke();
            return a;
        }
        else
            throw new System.InvalidOperationException("Tried adding non compatable types");
    }
    public static ItemSlot operator +(ItemSlot a, int b)
    {
        a.count += b;
        a.OnDataChange?.Invoke();
        a.OnCountChange?.Invoke();
        return a;
    }
    public static ItemSlot operator -(ItemSlot a, ItemSlot b)
    {
        if (a.item.SameItem(b.item))
        {
            a.count -= b.count;
            if (a.count <= 0)
                a.item = null;

            a.OnDataChange?.Invoke();
            a.OnCountChange?.Invoke();

            return a;
        }
        else
            throw new System.InvalidOperationException("Tried adding non compatable types");
    }
    public static ItemSlot operator -(ItemSlot a, int b)
    {
        a.count -= b;
        if (a.count <= 0)
            a.item = null;
        a.OnDataChange?.Invoke();
        a.OnCountChange?.Invoke();
        return a;

    }
    public static ItemSlot operator ++(ItemSlot a)
    {
        a.count++;
        if (a.count > 0)
        {
            a.OnDataChange?.Invoke();
        a.OnCountChange?.Invoke();
            return a;
        }
        else
            return null;
    }
    public static ItemSlot operator --(ItemSlot a)
    {
        a.count--;
        if (a.count <= 0)
            a.item = null;
        a.OnDataChange?.Invoke();
        a.OnCountChange?.Invoke();
        return a;
    }
}
public enum SlotType
{
    none,
    output,

    tool,
    weapon,


    ore,
    ingot,
    wood,

    part,
}

/*public class ItemSlotCompatability : IComparer<InvSlotType>
{
    public static int Compare(InventorySlot slot, Item item)
    {
        return Compare(slot.type, item.type);
    }
    public static int Compare(InvSlotType slot, ItemType item)
    {

    }
}*/
