using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public ResourceType type;
    public int amount;

    public bool IsEmpty => amount <= 0;
    public bool IsFull => amount >= 128;

    public InventorySlot(ResourceType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}

public class MJ_PlayerInventory : MonoBehaviour
{
    public static MJ_PlayerInventory Instance;

    [SerializeField] private List<InventorySlot> resources = new List<InventorySlot>();


    [SerializeField] private List<Image> images = new List<Image>();

    [SerializeField] private List<TextMeshProUGUI> inventoryNumberDisplay = new List<TextMeshProUGUI>();

    [SerializeField] GameObject inventoryGlobalLayout;

    [SerializeField] private Sprite woodSprite, stoneSprite, metalSprite, coalSprite, waterSprite, dieselSprite;
    private int numOfInventorySlots = 88;

    void Awake()
    {
            Instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var theImage in inventoryGlobalLayout.GetComponentsInChildren<Image>())
        {
            images.Add(theImage);

        }
        yield return new WaitForSeconds(.1f);
        foreach (var text in inventoryGlobalLayout.GetComponentsInChildren<TextMeshProUGUI>())
        {
            inventoryNumberDisplay.Add(text);
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        int remaining = amount;

        // Fill existing stacks first
        foreach (var slot in resources)
        {
            if (slot.type == type && !slot.IsFull)
            {
                int space = 128 - slot.amount;
                int toAdd = Mathf.Min(space, remaining);
                slot.amount += toAdd;
                remaining -= toAdd;
                if (remaining <= 0)
                {
                    AutoSortInventory();
                    UpdateResourcesUIText();
                    return;
                }
            }
        }

        // Create new stacks
        while (remaining > 0 && resources.Count < numOfInventorySlots)
        {
            int toAdd = Mathf.Min(128, remaining);
            resources.Add(new InventorySlot(type, toAdd));
            remaining -= toAdd;
        }

        if (remaining > 0)
            Debug.LogWarning($"❌ Inventory full! Could not add remaining {remaining}x {type}");

        AutoSortInventory();
        UpdateResourcesUIText();
    }


    private void MergeStacks(ResourceType type)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].type != type || resources[i].IsFull)
                continue;

            for (int j = i + 1; j < resources.Count; j++)
            {
                if (resources[j].type == type && !resources[j].IsEmpty)
                {
                    int space = 128 - resources[i].amount;
                    int transfer = Mathf.Min(space, resources[j].amount);

                    resources[i].amount += transfer;
                    resources[j].amount -= transfer;

                    if (resources[i].IsFull)
                        break;
                }
            }
        }

        // remove any now-empty slots
        resources.RemoveAll(s => s.IsEmpty);
    }

    public bool RemoveResource(ResourceType type, int amount)
    {
        int totalAvailable = GetResourceAmount(type);
        if (totalAvailable < amount)
        {
            Debug.Log($"❌ Not enough {type} to remove {amount}");
            return false;
        }

        int remaining = amount;

        foreach (var slot in resources)
        {
            if (slot.type == type && slot.amount > 0)
            {
                int toRemove = Mathf.Min(slot.amount, remaining);
                slot.amount -= toRemove;
                remaining -= toRemove;

                if (slot.amount <= 0) slot.amount = 0;
                if (remaining <= 0) break;
            }
        }

        // Remove empty slots at the end
        resources.RemoveAll(s => s.amount <= 0);
        AutoSortInventory();
        UpdateResourcesUIText();
        return true;
    }

    public void ClearInventory()
    {
        resources.Clear();
    }

    public int GetResourceAmount(ResourceType type)
    {
        int total = 0;
        foreach (var slot in resources)
        {
            if (slot.type == type)
                total += slot.amount;
        }
        return total;
    }


    void Update()
    {
        UpdateResourcesUIText();
    }

    void UpdateResourcesUIText()
    {
        /*for (int i = 0; i < images.Count; i++)
        {
            if (i < resources.Count && !resources[i].IsEmpty)
            {
                images[i].sprite = GetSpriteForType(resources[i].type);
                images[i].color = Color.white;
                inventoryNumberDisplay[i].text = resources[i].amount.ToString();
            }
            else
            {
                images[i].sprite = null;
                images[i].color = new Color(1, 1, 1, 0);
                inventoryNumberDisplay[i].text = "";
            }
        }*/

        // ensure no mismatch causes issues
        int slotCount = Mathf.Min(images.Count, inventoryNumberDisplay.Count);
        int resourceCount = resources.Count;

        for (int i = 0; i < slotCount; i++)
        {
            bool hasResource = i < resourceCount && resources[i] != null && !resources[i].IsEmpty;

            if (hasResource)
            {
                images[i].sprite = GetSpriteForType(resources[i].type);
                images[i].color = Color.white;
                inventoryNumberDisplay[i].text = resources[i].amount.ToString();
            }
            else
            {
                // clear slot if no resource or no matching UI element
                images[i].sprite = null;
                images[i].color = new Color(1, 1, 1, 0);
                inventoryNumberDisplay[i].text = "";
            }
        }
    }


    private Sprite GetSpriteForType(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood: return woodSprite;
            case ResourceType.Stone: return stoneSprite;
            case ResourceType.Metal: return metalSprite;
            case ResourceType.Coal: return coalSprite;
            case ResourceType.Water: return waterSprite;
            case ResourceType.Diesel: return dieselSprite;
            default: return null;
        }
    }

    private void AutoSortInventory()
    {
        // 1️⃣ Merge same-type stacks
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].IsEmpty || resources[i].IsFull) continue;

            for (int j = i + 1; j < resources.Count; j++)
            {
                if (resources[j].type == resources[i].type && !resources[j].IsEmpty)
                {
                    int space = 128 - resources[i].amount;
                    int transfer = Mathf.Min(space, resources[j].amount);

                    resources[i].amount += transfer;
                    resources[j].amount -= transfer;

                    if (resources[i].IsFull)
                        break;
                }
            }
        }

        // 2️⃣ Remove empty slots
        resources.RemoveAll(s => s.IsEmpty);

        // 3️⃣ Sort by resource type name for neatness
        resources.Sort((a, b) => a.type.CompareTo(b.type));
    }
}