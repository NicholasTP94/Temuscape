using UnityEngine;

public class PlayerToolVisual : MonoBehaviour
{
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private Transform toolSocket;

    private GameObject currentToolObject;

    private void Start()
    {
        if (playerEquipment == null)
        {
            playerEquipment = GetComponent<PlayerEquipment>();
        }

        playerEquipment.EquippedToolChanged += RefreshTool;

        RefreshTool(playerEquipment.EquippedTool);
    }

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

        currentToolObject = Instantiate(tool.HeldPrefab, toolSocket);

        currentToolObject.transform.localPosition = Vector3.zero;

        currentToolObject.transform.localRotation = Quaternion.identity;

        if (tool.HeldMaterialOverride != null)
        {
            Renderer[] renderers = currentToolObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = tool.HeldMaterialOverride;
            }
        }
    }

    private void OnDestroy()
    {
        if (playerEquipment != null)
        {
            playerEquipment.EquippedToolChanged -= RefreshTool;
        }
    }
}