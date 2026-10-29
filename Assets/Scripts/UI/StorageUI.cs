using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StorageUI: MonoBehaviour
{
    [SerializeField] private GameObject storagePanel;
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private Transform storageGrid;
    [SerializeField] private GameObject inventorySlotPrefab;

    [SerializeField] private Button closeButton;

    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InputControllerUI inputControllerUI;
    [SerializeField] private PlayerAnimator playerAnimator;

    private SharedStorage currentStorage;
    private GameObject[] storageSlotObjects;

    public SharedStorage CurrentStorage => currentStorage;
    public bool IsStorageOpen => storagePanel.activeSelf && currentStorage != null;


    private void Awake()
    {
        storagePanel.SetActive(false);

        CreateStorageSlots();
    }

    public void OpenStorage(SharedStorage storage)
    {
        if (storage == null)
        {
            return;
        }

        currentStorage = storage;

        storagePanel.SetActive(true);
        inventoryPanel.SetActive(true);

        inputControllerUI.UnlockedCursor();

        playerAnimator.SetInteraction(true);

        RefreshStorageInventory();
    }

    public void CloseStorage()
    {
        storagePanel.SetActive(false);
        inventoryPanel.SetActive(false);

        currentStorage = null;

        inputControllerUI.LockedCursor();

        playerAnimator.SetInteraction(false);
    }

    private void RefreshStorageInventory()
    {
        if (currentStorage == null)
        {
            return;
        }

        if (currentStorage == null)
        {
            return;
        }

        for (int i = 0; i < storageSlotObjects.Length; i++)
        {
            InventorySlot storageSlot = currentStorage.StorageInventory.Slots[i];

            GameObject uiSlot = storageSlotObjects[i];

            Button slotButton = uiSlot.GetComponent<Button>();

            slotButton.onClick.RemoveAllListeners();

            Image itemIcon = uiSlot.transform.Find("ItemIcon").GetComponent<Image>();

            TMP_Text quantityText = uiSlot.transform.Find("QuantityText").GetComponent<TMP_Text>();

            if (storageSlot.IsEmpty)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;

                quantityText.text = "";
            }
            else
            {
                itemIcon.sprite = storageSlot.Item.Icon;

                itemIcon.enabled = true;

                quantityText.text = storageSlot.Quantity.ToString();

                ItemDefinition item = storageSlot.Item;

                slotButton.onClick.AddListener(() => OnStorageSlotClicked(item));
            }
        }
    }

    private void CreateStorageSlots()
    {
        storageSlotObjects = new GameObject[40];

        for (int i = 0; i < storageSlotObjects.Length; i++)
        {
            storageSlotObjects[i] = Instantiate(inventorySlotPrefab, storageGrid);
        }
    }

    public void MoveItemToStorage(ItemDefinition item)
    {
        if (currentStorage == null || item == null)
        {
            return;
        }

        if (!currentStorage.StorageInventory.CanAddItem(item, 1))
        {
            return;
        }

        bool removed = playerInventory.Inventory.RemoveItem(item, 1);

        if (!removed)
        {
            return;
        }

        currentStorage.StorageInventory.AddItem(item, 1);

        RefreshStorageInventory();
    }

    private void MoveItemToPlayer(ItemDefinition item)
    {
        if (currentStorage == null || item == null)
        {
            return;
        }

        if (!playerInventory.Inventory.CanAddItem(item, 1))
        {
            return;
        }

        bool removed = currentStorage.StorageInventory.RemoveItem(item, 1);

        if (!removed)
        {
            return;
        }

        playerInventory.Inventory.AddItem(item, 1);

        RefreshStorageInventory();
    }

    public void MoveStackToStorage(ItemDefinition item)
    {
        if (currentStorage == null || item == null)
        {
            return;
        }

        int quantity = playerInventory.Inventory.GetItemQuantity(item);

        if (quantity <= 0)
        {
            return;
        }

        if (!currentStorage.StorageInventory.CanAddItem(item, quantity))
        {
            return;
        }

        bool removed = playerInventory.Inventory.RemoveItem(item, quantity);

        if (!removed)
        {
            return;
        }

        currentStorage.StorageInventory.AddItem(item, quantity);

        RefreshStorageInventory();
    }

    private void OnStorageSlotClicked(ItemDefinition item)
    {
        if (Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed)
        {
            MoveStackToPlayer(item);
        }
        else
        {
            MoveItemToPlayer(item);
        }
    }

    private void MoveStackToPlayer(ItemDefinition item)
    {
        if (currentStorage == null || item == null)
        {
            return;
        }

        int quantity =
            currentStorage.StorageInventory.GetItemQuantity(item);

        if (quantity <= 0)
        {
            return;
        }

        if (!playerInventory.Inventory.CanAddItem(item, quantity))
        {
            return;
        }

        bool removed = currentStorage.StorageInventory.RemoveItem(item, quantity);

        if (!removed)
        {
            return;
        }

        playerInventory.Inventory.AddItem(item, quantity);

        RefreshStorageInventory();
    }
}
