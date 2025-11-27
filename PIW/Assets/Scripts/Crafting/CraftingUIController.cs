using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingUIController : MonoBehaviour
{
    public static CraftingUIController Instance;

    private VisualElement root;          // Reference to UXML root
    private DropdownField recipeDropdown;
    private Label ingredientLabel;
    private Button craftButton;
    private Button closeButton;

    private Iinteractable currentStation;        // <-- your missing field
    private List<RecipeSO> currentRecipes;       // recipes passed in

    private void Awake()
    {
        Instance = this;

        // Load UIDocument
        var uiDoc = GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;

        // Start hidden
        root.style.display = DisplayStyle.None;

        // Link UI elements
        recipeDropdown = root.Q<DropdownField>("RecipeDropdown");
        ingredientLabel = root.Q<Label>("IngredientsLabel");
        craftButton = root.Q<Button>("CraftButton");
        closeButton = root.Q<Button>("CloseButton");

        craftButton.clicked += CraftSelectedRecipe;
        closeButton.clicked += () => root.style.display = DisplayStyle.None;
    }

    // ---------------------------------------------------------------------
    //  SHOW UI (called from Workbench / Campfire / Refinery)
    // ---------------------------------------------------------------------
    public void ShowUI(Iinteractable station, List<RecipeSO> recipes)
    {
        currentStation = station;
        currentRecipes = recipes;

        RefreshRecipeDropdown();                  // <-- your missing method
        root.style.display = DisplayStyle.Flex;
    }

    // ---------------------------------------------------------------------
    //  POPULATE DROPDOWN
    // ---------------------------------------------------------------------
    private void RefreshRecipeDropdown()
    {
        recipeDropdown.choices = new List<string>();

        foreach (var r in currentRecipes)
            recipeDropdown.choices.Add(r.recipeName);

        recipeDropdown.index = 0;

        UpdateIngredientDisplay();
        recipeDropdown.RegisterValueChangedCallback(evt => UpdateIngredientDisplay());
    }

    // ---------------------------------------------------------------------
    //  SHOW REQUIRED INGREDIENTS
    // ---------------------------------------------------------------------
    private void UpdateIngredientDisplay()
    {
        if (recipeDropdown.index < 0 || recipeDropdown.index >= currentRecipes.Count)
            return;

        var recipe = currentRecipes[recipeDropdown.index];
        var text = "Requires:\n";

        foreach (var ing in recipe.ingredients)
            text += $"{ing.amount} × {ing.type}\n";

        ingredientLabel.text = text;
    }

    // ---------------------------------------------------------------------
    //  CRAFTING
    // ---------------------------------------------------------------------
    private void CraftSelectedRecipe()
    {
        if (recipeDropdown.index < 0 || recipeDropdown.index >= currentRecipes.Count)
            return;

        var recipe = currentRecipes[recipeDropdown.index];
        var inv = MJ_PlayerInventory.Instance;

        // Check required items
        if (!inv.HasAllResources(recipe.ingredients))
        {
            Debug.Log("❌ Not enough materials!");
            return;
        }

        // Consume + give result
        inv.RemoveAllResources(recipe.ingredients);

        if (recipe.outputCategory == ItemCategory.Tool)
        {
            // Tools are non-stackable and may use durability
            MJ_PlayerInventory.Instance.AddResource(
                recipe.outputItem,
                recipe.outputAmount,
                recipe.toolDurability
            );
        }
        else if (recipe.outputCategory == ItemCategory.Consumable)
        {
            // Foods and other consumables simply stack normally
            MJ_PlayerInventory.Instance.AddResource(
                recipe.outputItem,
                recipe.outputAmount
            );
        }
        else // Material / Default
        {
            MJ_PlayerInventory.Instance.AddResource(
                recipe.outputItem,
                recipe.outputAmount
            );
        }

        Debug.Log($"✔ Crafted {recipe.outputAmount} × {recipe.outputItem}");
    }
}
