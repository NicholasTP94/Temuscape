using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Game/Crafting/Crafting Recipe")]
public class CraftingRecipeDefinition: ScriptableObject
{
    [SerializeField] private string recipeId;
    [SerializeField] private string displayName;

    [SerializeField] private CraftingStationType requiredStation;
    [SerializeField] private Craftingngridient[] ingredients;
    [SerializeField] private ItemDefinition resultItem;
    [SerializeField] private int resultQuanity = 1;

    public string RecipeId => recipeId;
    public string DisplayName => displayName;

    public CraftingStationType RequiredStation => requiredStation;
    public Craftingngridient[] Ingredients => ingredients;
    public ItemDefinition ResultItem => resultItem;
    public int ResultQuantity => resultQuanity;
}
