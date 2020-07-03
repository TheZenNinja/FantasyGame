using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] bool highlighted;
    [SerializeField] GameObject highlight;
    [SerializeField] Image rarityBG;
    [SerializeField] Image sprite;
    [SerializeField] Sprite bgSprite;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        highlight.SetActive(highlighted);

        if (item)
        {
            sprite.color = new Color(0.2f, 0.2f, 0.2f, 1);
            sprite.enabled = true;
            sprite.sprite = item.sprite;
            rarityBG.color = Item.GetColor(item.rarity);
        }
        else
        {
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

    private void OnValidate()
    {
        UpdateUI();
    }

    public void ClearItem()
    {
        item = null;
        UpdateUI();
    }
    public void SetItem(Item i)
    {
        item = i;
        UpdateUI();
    }

    public void SwitchHighlight(bool highlighted)
    {
        this.highlighted = highlighted;
        UpdateUI();
    }
}
