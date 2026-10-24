using UnityEngine;
using System.Collections.Generic;
public class Inventory
{
    private readonly List<InventorySlot> slots;
    private const int DefaultCapacity = 16;

    public IReadOnlyList<InventorySlot> Slots => slots;

    public Inventory()
    {
        slots = new List<InventorySlot>();

        for (int i =0; i< DefaultCapacity; i++)
        {
            slots.Add(new InventorySlot());
        }
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

        return addedQuantity;
    }
}
