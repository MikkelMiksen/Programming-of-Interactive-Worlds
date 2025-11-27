using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FoodSelectorController : MonoBehaviour
{
    public TextMeshProUGUI line1, line2, line3;
    public int selectedIndex = 0;

    List<(ResourceType type, int count)> foods = new();

    void Update()
    {
        RefreshFoods();
        Draw();
    }

    void RefreshFoods()
    {
        foods = MJ_PlayerInventory.Instance.GetConsumablesList();
        if (foods.Count == 0) selectedIndex = 0;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, Mathf.Max(0, foods.Count - 1));
    }

    void Draw()
    {
        // draw 3 lines around selectedIndex
        for (int i = -1; i <= 1; i++)
        {
            int idx = selectedIndex + i;
            string text = "";
            if (idx >= 0 && idx < foods.Count) text = $"{foods[idx].type} : {foods[idx].count}";
            if (i == -1) line1.text = text;
            else if (i == 0) line2.text = "▶ " + text; // center arrow
            else line3.text = text;
        }
    }

    public void CycleLeft() { selectedIndex = Mathf.Max(0, selectedIndex - 1); }
    public void CycleRight() { selectedIndex = Mathf.Min(foods.Count - 1, selectedIndex + 1); }

    public void ConsumeSelected()
    {
        if (foods.Count == 0) return;
        var item = foods[selectedIndex];
        // remove one unit from inventory
        if (MJ_PlayerInventory.Instance.RemoveResource(item.type, 1))
        {
            // apply effects: example
            MJ_PlayerStats01.Instance.EatMeal(); // implement PlayerStats
        }
    }
}