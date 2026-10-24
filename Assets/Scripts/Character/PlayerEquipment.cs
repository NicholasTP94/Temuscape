using UnityEngine;

public class PlayerEquipment:MonoBehaviour
{
    [SerializeField] private ToolDefinition startingTool;

    private ToolDefinition equippedTool;

    public ToolDefinition EquippedTool => equippedTool;

    private void Awake()
    {
        equippedTool = startingTool;
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
}
