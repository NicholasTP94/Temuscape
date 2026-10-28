using UnityEngine;

public class CraftingStation: MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingStationType stationType;

    [SerializeField] private CraftingRecipeDefinition[] availableRecipes;

    private PlayerCrafting playerCrafting;

    public CraftingStationType StationType => stationType;

    public CraftingRecipeDefinition[] AvailableRecipes => availableRecipes;

    private CraftingUI craftingUI;

    private void Start()
    {
        playerCrafting = FindAnyObjectByType<PlayerCrafting>();
        craftingUI = FindAnyObjectByType<CraftingUI>();
    }
    public string GetInteractionText()
    {
        return $"Use {stationType}";
    }

    public void Interact()
    {
        if (craftingUI == null)
        {
            Debug.LogWarning("CraftingUI not found");

            return;
        }

        craftingUI.OpenStation(this);
    }
}

