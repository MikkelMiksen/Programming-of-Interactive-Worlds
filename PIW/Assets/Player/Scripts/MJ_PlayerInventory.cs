using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MJ_PlayerInventory : MonoBehaviour
{
    public static MJ_PlayerInventory Instance { get; private set; }

    [SerializeField]
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    [SerializeField]
    private TextMeshProUGUI inventoryUItext;

    void Awake()
    {
            Instance = this;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;
        resources[type] += amount;

        Debug.Log($"Added {amount} {type}. Total: {resources[type]}");
    }

    public bool RemoveResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type) || resources[type] < amount)
            return false;

        resources[type] -= amount;
        Debug.Log($"Removed {amount} {type}. Remaining: {resources[type]}");
        return true;
    }

    public void ClearInventory()
    {
        resources.Clear();
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }

    void Update()
    {
        UpdateResourcesUIText();
    }

    void UpdateResourcesUIText()
    {
        string displayText = "";

        // Helper dictionary for short codes
        Dictionary<ResourceType, string> shortCodes = new Dictionary<ResourceType, string>()
        {
            { ResourceType.Wood, "WO" },
            { ResourceType.Stone, "ST" },
            { ResourceType.Metal, "ME" },
            { ResourceType.Coal, "CO" },
            { ResourceType.Water, "WA" },
            { ResourceType.Diesel, "DI" }
        };

        foreach (var kvp in shortCodes)
        {
            if (resources.ContainsKey(kvp.Key) && resources[kvp.Key] > 0)
            {
                displayText += $"{resources[kvp.Key]}:{kvp.Value}\n";
            }
        }

        inventoryUItext.text = displayText; // Or whatever text field you want to use
    }
}