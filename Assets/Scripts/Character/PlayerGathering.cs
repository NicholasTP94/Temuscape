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
        //Finds gathering feedback UI if not assigned
        if (gatherFeedbackUI == null)
        {
            gatherFeedbackUI = FindAnyObjectByType<GatherFeedbackUI>();
        }

        //Finds  Animator if not assigned
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<PlayerAnimator>();
        }
    }

    private void Update()
    {
        //Find the resource the player is currently looking at
        FindResourceNode();

        //Try to gather when gather action button is pressed
        if (gatherAction.action.WasPressedThisFrame())
        {
            TryGather();
        }
    }
    private void FindResourceNode()
    {
        //Reset current resource
        currentResourceNode = null;

        float closestDistance = Mathf.Infinity;

        //Finds all gathering objects around the player
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

            //Selects the closest resource inside the gathering angle/ cone
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
        //Stops multiple gathering actions at the same time
        if (isGathering)
        {
            return;
        }

        //Checks if there is a resource to gather
        if (currentResourceNode == null)
        {
            return;
        }

        ToolDefinition equippedTool = playerEquipment.EquippedTool;

        //Player needs an equipped tool to be able to gather
        if (equippedTool == null)
        {
            return;
        }

        //Checks if the equipped tool can gather this resource
        if (!currentResourceNode.CanGatherWith(equippedTool))
        {
            gatherFeedbackUI.ShowWrongTool(currentResourceNode.transform);

            return;
        }

        //Stores the resource and tool until the animation hits
        isGathering = true;

        pendingResource = currentResourceNode;
        pendingTool = equippedTool;

        //Starts the animation for gathering/ minig
        playerAnimator.PlayMining();
    }

    public void PerformGatherHit()
    {
        //Apply the gathering hit from the animation event
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
        //Finish gathering and reset stored data
        isGathering = false;

        pendingResource = null;
        pendingTool = null;
    }
}
