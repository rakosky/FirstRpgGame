
using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;

    public int quantity;

    public InventoryItem(ItemData data)
    {
        this.data = data;
    }
    public int RemoveStacks(int n) => quantity -= n;
    public int AddStacks(int n) => quantity += n;

    public bool IsMaxStacks() => quantity == data.maxStackSize;
}
