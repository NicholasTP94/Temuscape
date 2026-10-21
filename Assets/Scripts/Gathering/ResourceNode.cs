using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceNode: MonoBehaviour
{
    [SerializeField] private ResourceNodeDefinition definition;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public ResourceNodeDefinition Definition => definition;

    private void Awake()
    {
        currentHealth = definition.MaxHealth;
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
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
}
