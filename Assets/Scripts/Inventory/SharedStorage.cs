using UnityEngine;

public class SharedStorage : MonoBehaviour, IInteractable
{
    [SerializeField] private int capacity = 40;

    private Inventory storageInventory;
    public Inventory StorageInventory => storageInventory;

    private StorageUI storageUI;

    private void Awake()
    {
        storageInventory = new Inventory(capacity);
    }

    private void Start()
    {
        storageUI = FindAnyObjectByType<StorageUI>();

        if (storageUI == null)
        {
            Debug.LogError("StorageUI was not found in the scene.");
        }
    }
    public string GetInteractionText()
    {
        return "Open";
    }

    public void Interact()
    {
        if (storageUI == null)
        {
            Debug.LogError("Cannot open storage: StorageUI is null.");

            return;
        }

        Debug.Log("Opening Shared Storage");

        storageUI.OpenStorage(this);
    }
}