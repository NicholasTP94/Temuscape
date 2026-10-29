using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class InventoryUI: MonoBehaviour
{
    [SerializeField] private Transform inventoryGrid;
    [SerializeField] private GameObject inventorySlotPrefab;

    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InputActionReference openInventoryAction;

    [SerializeField] private InputControllerUI inputControllerUI;

    [SerializeField] private PlayerPlacement playerPlacement;

    [SerializeField] private PlayerAnimator playerAnimator;

    private GameObject[] slotObjects;

    private const int slotCount = 16;

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = FindAnyObjectByType<PlayerInventory>();
        }

        if (playerPlacement == null)
        {
            playerPlacement = FindAnyObjectByType<PlayerPlacement>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = FindAnyObjectByType<PlayerAnimator>();
        }

        CreateSlots();

        playerInventory.Inventory.InventoryChanged += RefreshInventory;

        RefreshInventory();

        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (openInventoryAction.action.WasPressedThisFrame())
        {
            ToggleInventory();
        }
    }

    private void CreateSlots()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        slotObjects = new GameObject[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            slotObjects[i] = Instantiate(inventorySlotPrefab, inventoryGrid);
        }
    }

    public void RefreshInventory()
    {
        if (playerInventory == null)
        {
            return;
        }

        for (int i = 0; i < slotObjects.Length; i++)
        {
            InventorySlot inventorySlot = playerInventory.Inventory.Slots[i];

            GameObject uiSlot = slotObjects[i];

            Button slotButton = uiSlot.GetComponent<Button>();

            slotButton.onClick.RemoveAllListeners();

            Image itemIcon = uiSlot.transform.Find("ItemIcon").GetComponent<Image>();
            
            TMP_Text quantityText = uiSlot.transform.Find("QuantityText").GetComponent<TMP_Text>();

            if (inventorySlot.IsEmpty)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;

                quantityText.text = "";
            }
            else
            {
                itemIcon.sprite = inventorySlot.Item.Icon;

                itemIcon.enabled = true;

                quantityText.text = inventorySlot.Quantity.ToString();

                ItemDefinition item = inventorySlot.Item;

                slotButton.onClick.AddListener(() => OnSlotClicked(item));
            }
        }
    }

    private void OnDestroy()
    {
        if (playerInventory != null && playerInventory.Inventory != null)
        {
            playerInventory.Inventory.InventoryChanged -= RefreshInventory;
        }
    }

    private void ToggleInventory()
    {
        bool shouldOpen = !inventoryPanel.activeSelf;

        inventoryPanel.SetActive(shouldOpen);

        if (shouldOpen)
        {
            RefreshInventory();

            inputControllerUI.UnlockedCursor();

            playerAnimator.SetInteraction(true);
        }
        else
        {
            inputControllerUI.LockedCursor();

            playerAnimator.SetInteraction(false);
        }
    }

    private void OnSlotClicked(ItemDefinition item)
    {
        if (item is not DeployableDefinition deployable)
        {
            return;
        }

        playerPlacement.StartPlacement(deployable);

        inventoryPanel.SetActive(false);

        inputControllerUI.LockedCursor();
    }
}