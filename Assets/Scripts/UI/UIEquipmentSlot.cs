using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = $"Equipment slot: {equipmentType}";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        Inventory.instance.UnequipItem(item);
    }
}
