using UnityEngine;

public class CraftingStation: MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingStationType stationType;

    [SerializeField] private CraftingRecipeDefinition[] availableRecipes;

    private PlayerCrafting playerCrafting;

    public CraftingStationType StationType => stationType;

    public CraftingRecipeDefinition[] AvailableRecpes => availableRecipes;


    private void Start()
    {
        playerCrafting = FindAnyObjectByType<PlayerCrafting>();
    }
    public string GetInteractionText()
    {
        return $"Use {stationType}";
    }

    public void Interact()
    {
        Debug.Log($"Opened {stationType}. " + $"{availableRecipes.Length} recipes available.");

        foreach (CraftingRecipeDefinition recipe in availableRecipes)
        {
            Debug.Log($"- {recipe.DisplayName}");
        }
    }
}

