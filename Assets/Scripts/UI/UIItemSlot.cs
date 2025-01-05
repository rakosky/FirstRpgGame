using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem item)
    {
        this.item = item;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.quantity > 1)
            {
                itemText.text = item.quantity.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemText.text = "";
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            var quantToRemove = Input.GetKey(KeyCode.LeftShift) 
                ? item.quantity
                : 1;
            Inventory.instance.RemoveItem(item.data, quantToRemove);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item);
        }
    }
}
