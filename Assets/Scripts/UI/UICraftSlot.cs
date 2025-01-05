using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{

    private void OnEnable()
    {
        UpdateSlot(item);
        //if (item == null)
        //    return;

        //itemText.text = "";
        //if(!Inventory.instance.CanCraftItem(item.data))
        //    itemImage.color = new Color(1,1,1,.5f);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.CraftItem(item.data);
    }
}
