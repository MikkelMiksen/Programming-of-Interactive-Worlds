using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Crafting")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public ResourceType outputItem;
    public int outputAmount = 1;

    [System.Serializable]
    public struct Ingredient
    {
        public ResourceType type;
        public int amount;
    }

    public List<Ingredient> ingredients = new();

    public WorkstationType requiredStation = WorkstationType.Workbench;
}