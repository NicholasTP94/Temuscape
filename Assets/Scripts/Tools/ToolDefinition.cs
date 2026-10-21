using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "Game/Items/Tool Definition")]
public class ToolDefinition: ItemDefinition
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ToolTier toolTier;
    [SerializeField] private int damage;

    public ToolType ToolType => toolType;
    public ToolTier ToolTier => toolTier;
    public int Damage => damage;
}