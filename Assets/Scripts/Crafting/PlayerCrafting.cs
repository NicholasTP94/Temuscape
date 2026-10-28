using UnityEngine;

public class PlayerCrafting: MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private PlayerEquipment playerEquipment;

    [SerializeField] private CraftingRecipeDefinition[] handRecipes;

    public CraftingRecipeDefinition[] HandRecipes => handRecipes;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }

        if (playerEquipment == null)
        {
            playerEquipment = GetComponent<PlayerEquipment>();
        }
    }
    public bool CanCraft(CraftingRecipeDefinition recipe, CraftingStationType station)
    {
        if (recipe == null)
        {
            return false;
        }
        
        if (recipe.RequiredStation != station)
        {
            return false;
        }

        foreach (Craftingngridient ingredient in recipe.Ingredients)
        {
            if (!playerInventory.Inventory.HasItem(ingredient.Item, ingredient.Quanity))
            {
                return false;
            }
        }

        if (!playerInventory.Inventory.CanAddItem(recipe.ResultItem, recipe.ResultQuantity))
        {
            return false;
        }

        return true;
    }

    public bool Craft(CraftingRecipeDefinition recipe, CraftingStationType station)
    {
        if (recipe == null)
        {
            Debug.LogWarning("Cannot craft because no recipe is selected.");

            return false;
        }

        if (!CanCraft(recipe, station))
        {
            Debug.Log($"Cannot craft {recipe.DisplayName}.");

            return false;
        }

        foreach (Craftingngridient ingredient in recipe.Ingredients)
        {
            playerInventory.Inventory.RemoveItem(ingredient.Item, ingredient.Quanity);
        }

        playerInventory.Inventory.AddItem(recipe.ResultItem, recipe.ResultQuantity);

        if (recipe.ReplaceTool != null && recipe.ResultItem is ToolDefinition craftedTool)
        {
            if (playerEquipment != null)
            {
                playerEquipment.ReplaceTool(recipe.ReplaceTool, craftedTool);
            }
            else
            {
                Debug.LogWarning("PlayerEquipment reference is missing in PlayerCrafting.");
            }
        }

        Debug.Log($"Crafted {recipe.ResultQuantity}x " + $"{recipe.ResultItem.DisplayName}");

        return true;
    }
}
