using UnityEngine;

public class CraftingStation: MonoBehaviour, IInteractable
{
   [SerializeField] private CraftingStationType stationType;

    public CraftingStationType StationType => stationType;

    public string GetInteractionText()
    {
        return $"Use {stationType}";
    }

    public void Interact()
    {
        Debug.Log($"Opened {stationType} crafting station.");
    }
}
