using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> materialInventory = new();
    public List<InventoryItem> equipmentInventory = new();
    public List<InventoryItem> equippedItems = new();

    [Header("Inventory UI")]
    [SerializeField] private Transform materialUISlotParent;
    [SerializeField] private Transform equipmentUISlotParent;
    [SerializeField] private Transform equippedItemUISlotParent;
    private UIItemSlot[] materialUISlots;
    private UIItemSlot[] equipUISlots;
    private UIEquipmentSlot[] equippedItemUISlots;
    public event Action<InventoryItem> onItemUnequipped;
    public event Action<InventoryItem> onItemEquipped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        materialUISlots = materialUISlotParent.GetComponentsInChildren<UIItemSlot>();
        equipUISlots = equipmentUISlotParent.GetComponentsInChildren<UIItemSlot>();
        equippedItemUISlots = equippedItemUISlotParent.GetComponentsInChildren<UIEquipmentSlot>();
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        var targetInventory = itemData.itemType switch
        {
            ItemType.Material => materialInventory,
            ItemType.Equipment or _ => equipmentInventory,
        };

        int remainingStacks = quantity;
        while (remainingStacks > 0)
        {
            var stack = targetInventory.FirstOrDefault(i => i.data.id == itemData.id && !i.IsMaxStacks()); // add to stacks found at the front of inventory first
            if(stack == null)
            {
                stack = new InventoryItem(itemData);
                targetInventory.Add(stack);
            }

            var stacksToAdd = Mathf.Min(remainingStacks, itemData.maxStackSize - stack.quantity);
            stack.AddStacks(stacksToAdd);
            remainingStacks -= stacksToAdd;
        }

        UpdateSlotUI();
    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        var targetInventory = itemData.itemType switch
        {
            ItemType.Material => materialInventory,
            ItemType.Equipment or _ => equipmentInventory,
        };

        int remainingStacks = quantity;
        while (remainingStacks > 0)
        {
            var stack = targetInventory.LastOrDefault(i => i.data.id == itemData.id) // remove stacks found at the end of inventory first
                ?? throw new System.Exception("Tried to remove item, but item not found!");

            var stacksToRemove = Mathf.Min(remainingStacks, stack.quantity);
            stack.RemoveStacks(stacksToRemove);
            if (stack.quantity == 0)
                targetInventory.Remove(stack);
            remainingStacks -= stacksToRemove;
        }

        UpdateSlotUI();
    }

    public void EquipItem(InventoryItem item)
    {
        var itemToReplace = equippedItems.FirstOrDefault(e => ((EquipmentItemData)e.data).equipmentType == ((EquipmentItemData)item.data).equipmentType);
        if (itemToReplace != null)
            UnequipItem(itemToReplace);
        
        equipmentInventory.Remove(item);
        equippedItems.Add(item);
        UpdateSlotUI();

        onItemEquipped?.Invoke(item);
    }

    public void UnequipItem(InventoryItem item)
    {
        equippedItems.Remove(item);
        equipmentInventory.Add(item);
        UpdateSlotUI();
        onItemUnequipped?.Invoke(item);
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < materialUISlots.Length; i++)
        {
            var item = materialInventory.ElementAtOrDefault(i);
            materialUISlots[i].UpdateSlot(item);
        }
        
        for (int i = 0; i < equipUISlots.Length; i++)
        {
            var item = equipmentInventory.ElementAtOrDefault(i);
            equipUISlots[i].UpdateSlot(item);
        }
                
        foreach (var slot in equippedItemUISlots)
        {
            var item = equippedItems.FirstOrDefault(i => ((EquipmentItemData)i.data).equipmentType == slot.equipmentType);
            slot.UpdateSlot(item);
        }
        for (int i = 0; i < equippedItemUISlots.Length; i++)
        {
            var item = equipmentInventory.ElementAtOrDefault(i);
            equipUISlots[i].UpdateSlot(item);
        }
    }

    public bool CanCraftItem(ItemData itemToCraft)
    {
        var reqItems = itemToCraft.craftDetails.requiredItems;
        foreach (var reqItem in reqItems)
        {
            var targetInventory = reqItem.itemData.itemType switch
            {
                ItemType.Equipment => equipmentInventory,
                ItemType.Material or _ => materialInventory,
            };

            if (targetInventory.Where(i => i.data.id == reqItem.itemData.id).Sum(i => i.quantity) < reqItem.quantity)
                return false;
        }
        return true;
    }

    public void CraftItem(ItemData itemToCraft)
    {
        if (!CanCraftItem(itemToCraft))
            return;

        foreach (var reqItem in itemToCraft.craftDetails.requiredItems)
        {
            RemoveItem(reqItem.itemData, reqItem.quantity); 
        }

        AddItem(itemToCraft, 1);
    }
}
