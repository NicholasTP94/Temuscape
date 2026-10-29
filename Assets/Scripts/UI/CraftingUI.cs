using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class CraftingUI: MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private TMP_Text headerText;

    [Header("Recipe List")]
    [SerializeField] private Transform recipeContent;
    [SerializeField] private GameObject recipeButtonPrefab;

    [Header("Recipe Details")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Transform ingredientsContainer;
    [SerializeField] private GameObject ingredientRowPrefab;
    [SerializeField] private Button craftButton;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    [Header("Player")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCrafting playerCrafting;
    [SerializeField] private InputActionReference openCraftingPanel;
    [SerializeField] private PlayerAnimator playerAnimator;

    [SerializeField] private InputControllerUI inputControllerUI;

    private CraftingRecipeDefinition selectedRecipe;
    private CraftingStation currentStation;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = FindAnyObjectByType<PlayerInventory>();
        }

        if (playerCrafting == null)
        {
            playerCrafting = FindAnyObjectByType<PlayerCrafting>();
        }

        if (playerAnimator == null)
        {
            playerAnimator =
                FindAnyObjectByType<PlayerAnimator>();
        }

        craftingPanel.SetActive(false);

        craftButton.interactable = false;

        closeButton.onClick.AddListener(CloseCrafting);
        craftButton.onClick.AddListener(CraftSelectedRecipe);
    }

    private void Update()
    {
        if (openCraftingPanel.action.WasCompletedThisFrame())
        {
            if (craftingPanel.activeSelf)
            {
                CloseCrafting();
            }
            else
            {
                OpenHandCrafting();
            }
        }
    }

    public void OpenStation(CraftingStation station)
    {
        if (station == null)
        {
            return;
        }

        currentStation = station;
        selectedRecipe = null;

        craftingPanel.SetActive(true);
        craftButton.interactable = false;

        headerText.text = station.StationType.ToString().ToUpper();

        PopulateRecippeList(station.AvailableRecipes);

        Debug.Log($"Opened crafting UI for {station.StationType}");

        inputControllerUI.UnlockedCursor();

        playerAnimator.SetInteraction(true);
    }

    public void CloseCrafting()
    {
        craftingPanel.SetActive(false);

        inputControllerUI.LockedCursor();

        playerAnimator.SetInteraction(false);
    }

    public void PopulateRecippeList(CraftingRecipeDefinition[] recipes)
    {
        foreach (Transform child in recipeContent)
        {
            Destroy(child.gameObject);
        }

        foreach (CraftingRecipeDefinition recipe in recipes)
        {
            if (recipe == null)
            {
                continue;
            }

            GameObject buttonObject = Instantiate(recipeButtonPrefab, recipeContent);

            TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();

            Button button = buttonObject.GetComponent<Button>();

            if (buttonText != null)
            {
                buttonText.text = recipe.DisplayName;
            }

            if (button != null)
            {
                button.onClick.AddListener(() => SelectRecipe(recipe));
            }
        }
    }

    private void SelectRecipe(CraftingRecipeDefinition recipe)
    {
        if (recipe == null)
        {
            return;
        }

        selectedRecipe = recipe;

        itemNameText.text = selectedRecipe.DisplayName;

        PopulateIngredients(selectedRecipe);

        CraftingStationType stationType;

        if (currentStation != null)
        {
            stationType = currentStation.StationType;
        }
        else
        {
            stationType = CraftingStationType.Hand;
        }

        bool canCraft = playerCrafting.CanCraft(selectedRecipe, stationType);

        craftButton.interactable = canCraft;

        Debug.Log($"Selected recipe: {selectedRecipe.DisplayName}");
    }

    private void PopulateIngredients(CraftingRecipeDefinition recipe)
    {
        foreach (Transform child in ingredientsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Craftingngridient ingredient in recipe.Ingredients)
        {
            if (ingredient == null || ingredient.Item == null)
            {
                continue;
            }

            GameObject rowObject = Instantiate(ingredientRowPrefab, ingredientsContainer);

            TMP_Text[] texts = rowObject.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 2)
            {
                texts[0].text = ingredient.Item.DisplayName;

                int ownedQuantity = playerInventory.Inventory.GetItemQuantity(ingredient.Item);

                texts[1].text = $"{ownedQuantity} / {ingredient.Quanity}";
            }
        }
    }

    private void CraftSelectedRecipe()
    {
        if (selectedRecipe == null)
        {
            return;
        }

        CraftingStationType stationType;

        if (currentStation != null)
        {
            stationType = currentStation.StationType;
        }
        else
        {
            stationType = CraftingStationType.Hand;
        }

            bool crafted = playerCrafting.Craft(selectedRecipe, stationType);

        if (!crafted)
        {
            return;
        }

        PopulateIngredients(selectedRecipe);

        craftButton.interactable = playerCrafting.CanCraft(selectedRecipe, stationType);
    }

    public void OpenHandCrafting()
    {
        if (playerCrafting == null)
        {
            return;
        }

        currentStation = null;
        selectedRecipe = null;

        craftingPanel.SetActive(true);

        headerText.text = "CRAFTING";

        craftButton.interactable = false;

        PopulateRecippeList(playerCrafting.HandRecipes);

        inputControllerUI.UnlockedCursor();

        playerAnimator.SetInteraction(true);
    }
}
