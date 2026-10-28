using UnityEngine;

[CreateAssetMenu(fileName = "New Deployable", menuName = "Game/Items/Deployable")]
public class DeployableDefinition : ItemDefinition
{
    [SerializeField] private GameObject prefab;

    public GameObject Prefab => prefab;
}