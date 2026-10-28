using UnityEngine;
using System.Collections.Generic;
using System;
public class Inventory
{
    private readonly List<InventorySlot> slots;
    private const int DefaultCapacity = 16;

    public IReadOnlyList<InventorySlot> Slots => slots;

    public event Action InventoryChanged;

    public Inventory()
    {
        slots = new List<InventorySlot>();

        for (int i =0; i< DefaultCapacity; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool CanAddItem(ItemDefinition item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            return false;
        }

        int remainingQuantity = quantity;

        foreach (InventorySlot slot in slots)
        {

            if (slot.IsEmpty)
            {
                remainingQuantity -= item.MaxStack;
            }
            else if (slot.Item == item && slot.Quantity < item.MaxStack)
            {
                remainingQuantity -= item.MaxStack - slot.Quantity;
            }

            if (remainingQuantity <= 0)
            {
                return true;
            }
        }

        return false;
    }

    public int AddItem(ItemDefinition item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            return 0;
        }

        int remainingQuantity = quantity;

        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty)
            {
                continue;
            }

            if (slot.Item != item)
            {
                continue;
            }

            if (slot.Quantity >= item.MaxStack)
            {
                continue;
            }

            int availableSpace = item.MaxStack - slot.Quantity;
            int ammountToAdd = System.Math.Min(remainingQuantity, availableSpace);

            slot.AddQuantity(ammountToAdd);
            remainingQuantity -= ammountToAdd;

            if (remainingQuantity <= 0) 
            {
                break;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (remainingQuantity <= 0)
            {
                break;
            }

            if (!slot.IsEmpty)
            {
                continue;
            }

            int ammountToAdd = System.Math.Min(remainingQuantity, item.MaxStack);

            slot.SetItem(item, ammountToAdd);

            remainingQuantity -= ammountToAdd;
        }

        int addedQuantity = quantity - remainingQuantity;

        if (addedQuantity > 0)
        {
            InventoryChanged?.Invoke();
        }

        return addedQuantity;
    }

    public int GetItemQuantity(ItemDefinition item)
    {
        if (item == null)
        {
            return 0;
        }

        int totalQuantity = 0;

        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty && slot.Item == item)
            {
                totalQuantity += slot.Quantity;
            }
        }

        return totalQuantity;
    }

    public bool HasItem(ItemDefinition item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            return false;
        }

        return GetItemQuantity(item) >= quantity;
    }

    public bool RemoveItem(ItemDefinition item, int quantity)
    {
        if (!HasItem(item, quantity))
        {
            return false;
        }

        int remainingQuantity = quantity;

        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty || slot.Item != item)
            {
                continue;
            }

            int amountToRemove = System.Math.Min(remainingQuantity, slot.Quantity);

            slot.RemoveQuantity(amountToRemove);

            remainingQuantity -= amountToRemove;

            if (remainingQuantity <= 0)
            {
                InventoryChanged?.Invoke();
                return true;
            }
        }

        return true;
    }
}