using UnityEngine;

public class PlayerCrafting: MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    private CraftingStationType currentStaton = CraftingStationType.Hand;

    public bool CanCraft(CraftingRecipeDefinition recipe)
    {
        if (recipe == null)
        {
            return false;
        }

        if (recipe.RequiredStation != currentStaton)
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

    public bool Craft(CraftingRecipeDefinition recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log($"Cannot craft {recipe.DisplayName}.");

            return false;
        }

        foreach (Craftingngridient ingredient in recipe.Ingredients)
        {
            playerInventory.Inventory.RemoveItem(ingredient.Item, ingredient.Quanity);
        }

        playerInventory.Inventory.AddItem(recipe.ResultItem, recipe.ResultQuantity);

        Debug.Log($"Crafted {recipe.ResultQuantity}x " + $"{recipe.ResultItem.DisplayName}");

        return true;
    }
}
