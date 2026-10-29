using UnityEngine;

[CreateAssetMenu(fileName = "New Torch", menuName = "Game/Items/Torch")]
public class TorchDefinition : ItemDefinition
{
    [SerializeField] private GameObject heldPrefab;
    [SerializeField] private float lifetime = 600f;

    public GameObject HeldPrefab => heldPrefab;
    public float Lifetime => lifetime;
}