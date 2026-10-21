using UnityEngine;

[CreateAssetMenu(fileName = "NewResourceNode", menuName = "Game/Gathering/Resource Node Definition")]
public class ResourceNodeDefinition: ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int maxHealth;
    [SerializeField] private ItemDefinition dropItem;
    [SerializeField] private int dropQuantity;

    public string DisplayName => displayName;
    public ResourceType ResourceType => resourceType;
    public int MaxHealth => maxHealth;
    public ItemDefinition DropItem => dropItem;
    public int DropQuantity => dropQuantity;
}
