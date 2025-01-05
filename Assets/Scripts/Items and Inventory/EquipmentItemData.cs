using Assets.Scripts.Stats;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Item/Equipment")]
public class EquipmentItemData : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    public Stats stats;

    public EquipmentItemData()
    {
        itemType = ItemType.Equipment;
    }

    public void ExecuteItemEffects(Transform effectPosition)
    {
        foreach (var effect in itemEffects) 
        {
            effect.ExecuteEffect(effectPosition);
        }
    }
}
