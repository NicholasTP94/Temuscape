using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment:MonoBehaviour
{
    [SerializeField] private ToolDefinition[] availableTools;
    [SerializeField] private int startinngToolIndex = 0;

    [SerializeField] private InputActionReference EquipTool1;
    [SerializeField] private InputActionReference EquipTool2;
    [SerializeField] private InputActionReference EquipTool3;

    [SerializeField] private PlayerInventory playerInventory;

    private ToolDefinition equippedTool;

    public ToolDefinition EquippedTool => equippedTool;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }
    }

    private void Start()
    {
        if (availableTools == null || availableTools.Length == 0)
        {
            Debug.LogWarning("No tools assigned to PlayerEquipment.");

            return;
        }

        if (startinngToolIndex < 0 || startinngToolIndex >= availableTools.Length)
        {
            Debug.LogWarning("Invalid starting tool index.");

            return;
        }

        EquipTool(availableTools[startinngToolIndex]);
    }

    private void Update()
    {
        if (EquipTool1.action.WasPressedThisFrame())
        {
            EquipToolByIndex(0);
        }

        if (EquipTool2.action.WasPressedThisFrame())
        {
            EquipToolByIndex(1);
        }

        if (EquipTool3.action.WasPressedThisFrame())
        {
            EquipToolByIndex(2);
        }
    }
    public void EquipTool(ToolDefinition newTool)
    {
        if (newTool == null)
        {
            return;
        }

        if (!playerInventory.Inventory.HasItem(newTool, 1))
        {
            Debug.Log($"Cannot equip {newTool.DisplayName}. " + $"Tool is not in inventory.");

            return;
        }

        equippedTool = newTool;

        Debug.Log($"Equipped tool: {equippedTool.DisplayName}");
    }

    public void EquipToolByIndex(int index)
    {
        if (index < 0 || index >= availableTools.Length)
        {
            return;
        }

        EquipTool(availableTools[index]);
    }

    public void ReplaceTool(ToolDefinition oldTool, ToolDefinition newTool)
    {
        if (oldTool == null || newTool == null)
        {
            return;
        }

        for (int i=0; i < availableTools.Length; i++)
        {
            if (availableTools[i] == oldTool)
            {
                availableTools[i] = newTool;

                if (equippedTool == oldTool)
                {
                    EquipTool(newTool);
                }

                Debug.Log($"Replaced {oldTool.DisplayName} " + $"with {newTool.DisplayName}");

                return;
            }
        }
    }
}
