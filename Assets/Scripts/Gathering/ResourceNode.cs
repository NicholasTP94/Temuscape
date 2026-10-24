using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceNode: MonoBehaviour
{
    [SerializeField] private ResourceNodeDefinition definition;

    [SerializeField] private ToolDefinition testTool;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public ResourceNodeDefinition Definition => definition;

    private void Awake()
    {
        currentHealth = definition.MaxHealth;
    }

    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Hit(testTool);
        }
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
        Debug.Log($"{definition.DisplayName} destroyed. " + $"Dropped {definition.DropQuantity} " + $"{definition.DropItem.DisplayName}");

        Destroy(gameObject);
    }

    public void Hit(ToolDefinition tool)
    {
        if (tool == null)
        {
            return;
        }

        if (tool.ToolType != definition.RequieredToolType)
        {
            Debug.Log($"{tool.DisplayName} cannot gather " + $"{definition.DisplayName}. Wrong tool type.");

            return;
        }

        if (tool.ToolTier < definition.RequieredToolTier)
        {
            Debug.Log($"{tool.DisplayName} is not high enough tier " + $"for {definition.DisplayName}.");

            return;
        }

        TakeDamage(tool.Damage);
    }
}