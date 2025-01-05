using System;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Material Data", menuName = "Data/Item/Material")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int id;
    public new string name;
    public Sprite icon;
    public int maxStackSize = 1;

    public ItemCraftDetails craftDetails;

}