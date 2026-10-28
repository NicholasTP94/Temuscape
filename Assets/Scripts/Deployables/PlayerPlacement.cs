using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlacement : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private InputActionReference placeAction;
    [SerializeField] private InputActionReference cancelPlacementAction;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float placementDistance = 4f;

    private DeployableDefinition currentDeployable;
    private GameObject previewObject;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }
    }

    private void Update()
    {
        if (currentDeployable == null)
        {
           return;
        }

        UpdatePreviewPosition();

        if (placeAction.action.WasPressedThisFrame())
        {
            Place();
        }

        if (cancelPlacementAction.action.WasPressedThisFrame())
        {
            CancelPlacement();
        }
    }

    public void StartPlacement(DeployableDefinition deployable)
    {
        if (deployable == null)
        {
            return;
        }

        if (!playerInventory.Inventory.HasItem(deployable, 1))
        {
            return;
        }

        CancelPlacement();

        currentDeployable = deployable;

        previewObject = Instantiate(currentDeployable.Prefab);

        DisablePreviewComponents();
    }

    private void UpdatePreviewPosition()
    {
        Vector3 rayStart = transform.position + Vector3.up * 2f;

        Vector3 rayDirection = transform.forward;

        Vector3 targetPosition = rayStart + rayDirection * placementDistance;

        if (Physics.Raycast(targetPosition, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            previewObject.transform.position = hit.point;
        }
    }

    private void Place()
    {
        if (previewObject == null || currentDeployable == null)
        {
            return;
        }

        Vector3 placementPosition = previewObject.transform.position;

        Quaternion placementRotation = previewObject.transform.rotation;

        Instantiate(currentDeployable.Prefab, placementPosition, placementRotation);

        playerInventory.Inventory.RemoveItem(currentDeployable, 1);

        Destroy(previewObject);

        previewObject = null;
        currentDeployable = null;
    }

    private void CancelPlacement()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = null;
        currentDeployable = null;
    }

    private void DisablePreviewComponents()
    {
        Collider[] colliders = previewObject.GetComponentsInChildren<Collider>();

        foreach (Collider colider in colliders)
        {
            colider.enabled = false;
        }

        CraftingStation[] craftingStations = previewObject.GetComponentsInChildren<CraftingStation>();

        foreach (CraftingStation station in craftingStations)
        {
            station.enabled = false;
        }
    }
}