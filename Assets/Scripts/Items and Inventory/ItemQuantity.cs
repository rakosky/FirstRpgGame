using System;

[Serializable]
public class ItemQuantity
{
    public ItemData itemData;
    public int quantity = 1;

    public ItemQuantity(ItemData itemData, int quantity)
    {
        this.quantity = quantity;
        this.itemData = itemData;
    }
}