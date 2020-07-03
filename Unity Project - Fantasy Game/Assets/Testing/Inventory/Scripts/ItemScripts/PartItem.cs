using System;
using System.Collections.Generic;
using UnityEngine;

public class PartItem : Item
{
    public Part part;

    public PartItem(Part p, Sprite s = null)
    {
        part = p;
        itemName = p.name;
        stackable = false;
        description = part.ToString();
        if (s == null)
            sprite = ItemSpriteAtlas.instance.GetPartSprite(p.type);
    }
}