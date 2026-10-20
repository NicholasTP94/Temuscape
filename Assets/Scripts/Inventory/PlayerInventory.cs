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
}
