using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    [SerializeField] private int testQuantity;

    private Inventory inventory;
    public Inventory Inventory => inventory;

    private void Awake()
    {
        inventory = new Inventory();
    }
}
