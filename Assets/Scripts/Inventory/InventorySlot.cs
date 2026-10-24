using UnityEngine;

public class InventorySlot
{
    private ItemDefinition item;
    private int quantity;

    public ItemDefinition Item => item;
    public int Quantity => quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public void SetItem(ItemDefinition newItem, int newQuantity)
    {
        if (newItem == null || newQuantity <=0)
        {
            Clear();
            return;
        }

        item = newItem;
        quantity = newQuantity;
    }

    public void AddQuantity(int ammount)
    {
        if (ammount <=0)
        {
            return;
        }

        quantity += ammount;
    }

    public void RemoveQuantity(int ammount)
    {
        if (ammount <= 0)
        {
            return;
        }

        quantity -= ammount;

        if (ammount <= 0)
        {
            Clear();
        }
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
