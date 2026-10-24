using Unity.VisualScripting;
using UnityEngine;

public class TestInteractable: MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Interact with Test Object";
    }
    public void Interact()
    {
        Debug.Log("Interaction successful!");
    }
}
