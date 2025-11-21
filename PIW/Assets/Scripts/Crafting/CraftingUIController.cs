using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingUIController : MonoBehaviour
{
    public static CraftingUIController Instance;

    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private List<RecipeSO> availableRecipes = new();

    private DropdownField recipeDropdown;
    private Label ingredientsLabel;
    private Button craftButton;
    private Button closeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var root = uiDocument.rootVisualElement;

        recipeDropdown = root.Q<DropdownField>("RecipeDropdown");
        ingredientsLabel = root.Q<Label>("IngredientsLabel");
        craftButton = root.Q<Button>("CraftButton");
        closeButton = root.Q<Button>("CloseButton");

        root.style.display = DisplayStyle.None;

        root.schedule.Execute(() => {
            InitDropdown();
            HookEvents();
        });
    }

    void InitDropdown()
    {
        recipeDropdown.choices = new List<string>();

        foreach (var recipe in availableRecipes)
        {
            recipeDropdown.choices.Add(recipe.recipeName);
        }

        if (recipeDropdown.choices.Count > 0)
            recipeDropdown.index = 0;

        UpdateIngredientPreview();
    }

    void HookEvents()
    {
        recipeDropdown.RegisterValueChangedCallback(evt =>
        {
            UpdateIngredientPreview();
        });

        craftButton.clicked += CraftSelectedRecipe;
        closeButton.clicked += () => ShowUI(false);
    }

    void UpdateIngredientPreview()
    {
        if (recipeDropdown.index < 0) return;

        var recipe = availableRecipes[recipeDropdown.index];

        string text = "Requires:\n";
        foreach (var ing in recipe.ingredients)
            text += $"{ing.type}: {ing.amount}\n";

        ingredientsLabel.text = text;
    }

    void CraftSelectedRecipe()
    {
        var recipe = availableRecipes[recipeDropdown.index];

        if (MJ_PlayerInventory.Instance.RemoveAllResources(recipe.ingredients))
        {
            MJ_PlayerInventory.Instance.AddResource(recipe.outputItem, recipe.outputAmount);
        }
        else
        {
            Debug.Log("❌ Not enough materials!");
        }
    }

    public void ShowUI(bool show)
    {
        uiDocument.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
