using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    [SerializeField] private int testQuantity;

    [SerializeField] private ItemDefinition startingAxe;
    [SerializeField] private ItemDefinition startingPickaxe;

    private Inventory inventory;
    public Inventory Inventory => inventory;

    private void Awake()
    {
        inventory = new Inventory();

        inventory.AddItem(startingAxe, 1);
        inventory.AddItem(startingPickaxe, 1);
    }
}
