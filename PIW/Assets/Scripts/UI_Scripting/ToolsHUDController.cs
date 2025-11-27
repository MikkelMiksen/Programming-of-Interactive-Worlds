using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolsHUDController : MonoBehaviour
{
    public List<Image> toolSlots; // assign in Inspector
    public Sprite GetSpriteForType(ResourceType t) => /* reuse MJ_PlayerInventory.GetSpriteForType */ null;

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        var tools = MJ_PlayerInventory.Instance.GetToolsList();
        for (int i = 0; i < toolSlots.Count; i++)
        {
            if (i < tools.Count)
            {
                toolSlots[i].sprite = MJ_PlayerInventory.Instance.GetSpriteForType(tools[i].type);
                toolSlots[i].color = Color.white;
            }
            else
            {
                toolSlots[i].sprite = null;
                toolSlots[i].color = new Color(1,1,1,0);
            }
        }
    }
}