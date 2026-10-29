using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "Game/Items/Tool Definition")]
public class ToolDefinition: ItemDefinition
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ToolTier toolTier;
    [SerializeField] private int damage;
    [SerializeField] private GameObject heldPrefab;
    [SerializeField] private Material heldMaterialOverride;

    public ToolType ToolType => toolType;
    public ToolTier ToolTier => toolTier;
    public int Damage => damage;
    public GameObject HeldPrefab => heldPrefab;
    public Material HeldMaterialOverride => heldMaterialOverride;
}