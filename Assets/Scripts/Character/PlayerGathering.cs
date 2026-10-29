using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGathering: MonoBehaviour
{
    [SerializeField] private PlayerEquipment playerEquipment;

    [SerializeField] private Transform gatheringOrigin;
    [SerializeField] private float gatheringDistance;
    [SerializeField] private LayerMask gatheringLayer;

    [SerializeField] private InputActionReference gatherAction;
    [SerializeField] private float gatheringAngle = 90f;

    [SerializeField] private PlayerAnimator playerAnimator;

    [SerializeField] private GatherFeedbackUI gatherFeedbackUI;

    private ResourceNode currentResourceNode;

    private bool isGathering;
    private ResourceNode pendingResource;
    private ToolDefinition pendingTool;

    private void Awake()
    {
        if (gatherFeedbackUI == null)
        {
            gatherFeedbackUI = FindAnyObjectByType<GatherFeedbackUI>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<PlayerAnimator>();
        }
    }

    private void Update()
    {
        FindResourceNode();

        if (gatherAction.action.WasPressedThisFrame())
        {
            TryGather();
        }
    }
    private void FindResourceNode()
    {
        currentResourceNode = null;

        float closestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(gatheringOrigin.position, gatheringDistance, gatheringLayer);

        foreach (Collider collider in colliders) 
        {
            Vector3 clossestPoint = collider.ClosestPoint(gatheringOrigin.position);

            Vector3 directionToTarget = clossestPoint - gatheringOrigin.position;

            float distance = directionToTarget.magnitude;

            if (distance <= 0.001f)
            {
                directionToTarget = collider.bounds.center - gatheringOrigin.position;

                distance = directionToTarget.magnitude;
            }

            if (distance <= 0.001f)
            {
                directionToTarget = gatheringOrigin.forward;
            }
            else
            {
                directionToTarget.Normalize();
            }

            float angle = Vector3.Angle(gatheringOrigin.forward, directionToTarget);

            if (angle < gatheringAngle * 0.5f && distance < closestDistance)
            {
                ResourceNode resourceNode = collider.GetComponentInParent<ResourceNode>();

                if (resourceNode != null)
                {
                    closestDistance = distance;
                    currentResourceNode = resourceNode;
                }
            }
        }
    }

    private void TryGather()
    {
        if (isGathering)
        {
            return;
        }

        if (currentResourceNode == null)
        {
            return;
        }

        ToolDefinition equippedTool = playerEquipment.EquippedTool;

        if (equippedTool == null)
        {
            return;
        }

        if (!currentResourceNode.CanGatherWith(equippedTool))
        {
            gatherFeedbackUI.ShowWrongTool(currentResourceNode.transform);

            return;
        }

        isGathering = true;

        pendingResource = currentResourceNode;
        pendingTool = equippedTool;

        playerAnimator.PlayMining();
    }

    public void PerformGatherHit()
    {
        if (!isGathering)
        {
            return;
        }

        if (pendingResource == null || pendingTool == null)
        {
            return;
        }

        pendingResource.Hit(pendingTool);
    }

    public void FinishGathering()
    {
        isGathering = false;

        pendingResource = null;
        pendingTool = null;
    }
}
