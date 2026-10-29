using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceNode: MonoBehaviour
{
    [SerializeField] private ResourceNodeDefinition definition;
    [SerializeField] private ItemPickup itemPickupPrefab;
    [SerializeField] private Transform dropPoint;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float dropGroundOffset = 0.15f;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public ResourceNodeDefinition Definition => definition;

    private void Awake()
    {
        currentHealth = definition.MaxHealth;
    }

    private void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log($"{definition.DisplayName} took {damage} damage. " + $"HP: {currentHealth}/{definition.MaxHealth}");

        if (currentHealth <= 0)
        {
            BreakNode();
        }
    }

    private void BreakNode()
    {
        Vector3 spawnPosition = dropPoint.position;

        Vector3 rayStart = dropPoint.position + Vector3.up * 2f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 5f, groundLayer))
        {
            spawnPosition = hit.point + Vector3.up * dropGroundOffset;
        }

        ItemPickup pickup = Instantiate(itemPickupPrefab, spawnPosition, Quaternion.identity);

        pickup.Innitialize(definition.DropItem, definition.DropQuantity);

        Debug.Log($"{definition.DisplayName} destroyed. " + $"Dropped {definition.DropQuantity} " + $"{definition.DropItem.DisplayName}");

        Destroy(gameObject);
    }

    public void Hit(ToolDefinition tool)
    {
        if (!CanGatherWith(tool))
        {
            Debug.Log($"{tool?.DisplayName} cannot gather {definition.DisplayName}.");

            return;
        }

        TakeDamage(tool.Damage);
    }

    public bool CanGatherWith(ToolDefinition tool)
    {
        if (tool == null)
        {
            return false;
        }

        if (tool.ToolType != definition.RequieredToolType)
        {
            return false;
        }

        if (tool.ToolTier < definition.RequieredToolTier)
        {
            return false;
        }

        return true;
    }
}