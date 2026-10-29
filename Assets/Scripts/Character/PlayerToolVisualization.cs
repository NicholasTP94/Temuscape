using NUnit.Framework;
using UnityEngine;

public class PlayerToolVisual : MonoBehaviour
{
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private Transform toolSocket;

    private GameObject currentToolObject;

    private void Start()
    {
        // Connects to the equipment system
        if (playerEquipment == null)
        {
            playerEquipment = GetComponent<PlayerEquipment>();
        }

        //shows the currently equipped tool.
        playerEquipment.EquippedToolChanged += RefreshTool;

        RefreshTool(playerEquipment.EquippedTool);
    }

    // Updates the tool model shown in the player's hand.
    // Removes the old tool and creates the new equipped tool.
    private void RefreshTool(ToolDefinition tool)
    {
        if (currentToolObject != null)
        {
            Destroy(currentToolObject);
        }

        currentToolObject = null;

        if (tool == null || tool.HeldPrefab == null)
        {
            return;
        }

        //Placing the tool to the socket
        currentToolObject = Instantiate(tool.HeldPrefab, toolSocket);

        currentToolObject.transform.localPosition = Vector3.zero;

        currentToolObject.transform.localRotation = Quaternion.identity;

        // Changes the material if this tool has a material override.
        if (tool.HeldMaterialOverride != null)
        {
            Renderer[] renderers = currentToolObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = tool.HeldMaterialOverride;
            }
        }
    }

    // Stops listening to the equipment event
    private void OnDestroy()
    {
        if (playerEquipment != null)
        {
            playerEquipment.EquippedToolChanged -= RefreshTool;
        }
    }
}