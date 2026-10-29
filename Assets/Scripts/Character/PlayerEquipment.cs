using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerEquipment:MonoBehaviour
{
    [SerializeField] private ToolDefinition[] availableTools;
    [SerializeField] private int startinngToolIndex = 0;

    [SerializeField] private InputActionReference EquipTool1;
    [SerializeField] private InputActionReference EquipTool2;
    [SerializeField] private InputActionReference EquipTool3;

    [SerializeField] private PlayerInventory playerInventory;

    private ToolDefinition equippedTool;

    public event Action<ToolDefinition> EquippedToolChanged;

    public ToolDefinition EquippedTool => equippedTool;

    private void Awake()
    {
        //Finds PlayerInventory if not assigned
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }
    }

    private void Start()
    {
        //Checks if tools are assigned
        if (availableTools == null || availableTools.Length == 0)
        {
            Debug.LogWarning("No tools assigned to PlayerEquipment.");

            return;
        }

        //Checks if starting tool index is valid
        if (startinngToolIndex < 0 || startinngToolIndex >= availableTools.Length)
        {
            Debug.LogWarning("Invalid starting tool index.");

            return;
        }

        //Equips starting tool
        EquipTool(availableTools[startinngToolIndex]);
    }

    //Tool Equipment
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
    }

    public void EquipTool(ToolDefinition newTool)
    {
        //Equips a tool only if the player has it in inventory
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

        EquippedToolChanged?.Invoke(equippedTool);

        Debug.Log($"Equipped tool: {equippedTool.DisplayName}");
    }

    public void EquipToolByIndex(int index)
    {
        //Equips tool from available tools list
        if (index < 0 || index >= availableTools.Length)
        {
            return;
        }

        EquipTool(availableTools[index]);
    }

    public void ReplaceTool(ToolDefinition oldTool, ToolDefinition newTool)
    {
        //Replaces an old tool with a new upgraded tool
        if (oldTool == null || newTool == null)
        {
            return;
        }

        for (int i=0; i < availableTools.Length; i++)
        {
            if (availableTools[i] == oldTool)
            {
                availableTools[i] = newTool;

                //Equips the new tool if the old one was currently equipped
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
