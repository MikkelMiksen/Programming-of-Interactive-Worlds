using UnityEngine;
using UnityEngine.EventSystems;

public class WoodPile : MonoBehaviour, Iinteractable
{

    [SerializeField]
    private int woodAmount;

    public void Interact()
    {
        MJ_PlayerInventory.Instance.AddResource(ResourceType.Wood, woodAmount);
    }

    public string GetPrompt() => "Press E to gather wood";
}
