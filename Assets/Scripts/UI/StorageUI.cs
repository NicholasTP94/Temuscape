using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        storagePanel.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseStorage);
        }

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
}
