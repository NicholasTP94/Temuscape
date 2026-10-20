using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup: MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition itemDefinition;
    [SerializeField] private int quantity = 1;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public string GetInteractionText()
    {
        return $"Pick up {itemDefinition.DisplayName} x{quantity}";
    }

    public void Interact()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("No Player Inventory Found");
            return;
        }

        int addedAmount = playerInventory.Inventory.AddItem(itemDefinition, quantity);

        if (addedAmount <=0)
        {
            Debug.Log("Inventory full.");
            return;
        }

        quantity -= addedAmount;

        Debug.Log($"Picked up {addedAmount} " + $"{itemDefinition.DisplayName}. " + $"{quantity} remaining.");

        if (quantity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
