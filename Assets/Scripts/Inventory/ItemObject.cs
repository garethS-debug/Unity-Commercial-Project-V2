using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;
    public string description;
}

public enum ItemType
{
    Key,
    Heart,
    PuzzlePiece,
    Default
}

[Serializable]
public class Item
{
    public string Name;
    public int Id;

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
    }
}