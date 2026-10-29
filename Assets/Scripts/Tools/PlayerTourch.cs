using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTorch : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TorchDefinition torchDefinition;
    [SerializeField] private Transform torchSocket;
    [SerializeField] private InputActionReference equipTorchAction;

    private GameObject activeTorch;
    private float remainingLifetime;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }
    }

    private void Update()
    {
        if (equipTorchAction.action.WasPressedThisFrame())
        {
            ToggleTorch();
        }

        if (activeTorch != null)
        {
            UpdateTorchLifetime();
        }
    }

    private void ToggleTorch()
    {
        if (activeTorch != null)
        {
            DestroyTorch();
            return;
        }

        EquipTorch();
    }

    private void EquipTorch()
    {
        if (torchDefinition == null)
        {
            return;
        }

        if (!playerInventory.Inventory.HasItem(torchDefinition,1))
        {
            return;
        }

        bool removed = playerInventory.Inventory.RemoveItem(torchDefinition, 1);

        if (!removed)
        {
            return;
        }

        activeTorch = Instantiate(torchDefinition.HeldPrefab, torchSocket);

        activeTorch.transform.localPosition = Vector3.zero;

        activeTorch.transform.localRotation = Quaternion.identity;

        remainingLifetime = torchDefinition.Lifetime;
    }

    private void UpdateTorchLifetime()
    {
        remainingLifetime -= Time.deltaTime;

        if (remainingLifetime <= 0f)
        {
            DestroyTorch();
        }
    }

    private void DestroyTorch()
    {
        if (activeTorch != null)
        {
            Destroy(activeTorch);
        }

        activeTorch = null;
        remainingLifetime = 0f;
    }
}