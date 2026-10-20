using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory: MonoBehaviour
{
    [SerializeField] private ItemDefinition testItem;
    [SerializeField] private int testQuantity;

    private Inventory inventory;
    public Inventory Inventory => inventory;

    private void Awake()
    {
        inventory = new Inventory();
    }

    //boulo

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            FillInventoryForTest();
        }
    }

    private void FillInventoryForTest()
    {
        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            InventorySlot slot = inventory.Slots[i];

            if (slot.IsEmpty)
            {
                slot.SetItem(
                    testItem,
                    testItem.MaxStack
                );
            }
        }

        InventorySlot lastSlot =
            inventory.Slots[inventory.Slots.Count - 1];

        lastSlot.SetItem(
            testItem,
            testItem.MaxStack - 1
        );

        PrintInventory();
    }
    private void PrintInventory()
    {
        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            InventorySlot slot = inventory.Slots[i];

            if (slot.IsEmpty)
            {
                Debug.Log($"Slot {i}: Empty");
            }
            else
            {
                Debug.Log(
                    $"Slot {i}: " +
                    $"{slot.Item.DisplayName} x{slot.Quantity}"
                );
            }
        }
    }
}
