using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Items/Item Definition")]
public class ItemDefinition: ScriptableObject
{
    [SerializeField] private string itemId;
    [SerializeField] private string displayName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStack = 5;
    [SerializeField] private ItemType itemType;

    public string ItemId => itemId;
    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public int MaxStack => maxStack;
    public ItemType ItemType => itemType;
}
