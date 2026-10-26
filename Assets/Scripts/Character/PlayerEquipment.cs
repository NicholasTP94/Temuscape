using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment:MonoBehaviour
{
    [SerializeField] private ToolDefinition[] availableTools;
    [SerializeField] private int startinngToolIndex = 0;

    [SerializeField] private InputActionReference EquipTool1;
    [SerializeField] private InputActionReference EquipTool2;
    [SerializeField] private InputActionReference EquipTool3;

    private ToolDefinition equippedTool;

    public ToolDefinition EquippedTool => equippedTool;

    private void Awake()
    {
        if (availableTools == null || availableTools.Length == 0)
        {
            Debug.LogWarning("No tools assigned");
            return;
        }

        if (startinngToolIndex <0 || startinngToolIndex >= availableTools.Length)
        {
            startinngToolIndex = 0;
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
}
