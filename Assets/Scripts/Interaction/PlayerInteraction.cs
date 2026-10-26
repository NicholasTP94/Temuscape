using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private float interactionAngle = 60f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private InputActionReference interactionAction;
    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float closeInteractionRadius = 1f;

    private IInteractable currentInteractable;

    private void Update()
    {
        FindInteractable();

        if (
            interactionAction.action.WasPressedThisFrame() && currentInteractable != null
        )
        {
            currentInteractable.Interact();
        }
    }

    private void FindInteractable()
    {
        IInteractable previusInteractable = currentInteractable;

        IInteractable bestInteractable = null;

        float closestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(interactionOrigin.position, interactionDistance, interactionLayer);

        foreach (Collider collider in colliders)
        {
            Vector3 clossestPoint = collider.ClosestPoint(interactionOrigin.position);

            Vector3 directionToTarget = clossestPoint - interactionOrigin.position;

            float distance = directionToTarget.magnitude;

            if (distance <= 0.001f)
            {
                continue;
            }

            directionToTarget.Normalize();

            float angle = Vector3.Angle(interactionOrigin.forward, directionToTarget);

            Vector3 horizontalDirection = collider.bounds.center - transform.position;

            horizontalDirection.y = 0f;

            float horizontalDistance = horizontalDirection.magnitude;

            bool isCloseEnough = horizontalDistance <= horizontalDirection.magnitude;

            bool isInsideAngle = angle <= interactionAngle * 0.5f;

            if ((isCloseEnough || isInsideAngle) && distance < closestDistance)
            {
                if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    closestDistance = distance;
                    bestInteractable = interactable;
                }
            }
        }

        currentInteractable = bestInteractable;

        LogTargetChange(previusInteractable, currentInteractable);
    }

    private void LogTargetChange(IInteractable previousInteractable, IInteractable newInteractable)
    {
        if (previousInteractable == newInteractable)
        {
            return;
        }
        if (newInteractable != null)
        {
            Debug.Log("Target Found" + newInteractable.GetInteractionText());
        }
        else
        {
            Debug.Log("Target Lost");
        }
    }

    private void OnDrawGizmos()
    {
        if (interactionOrigin == null)
        {
            return;
        }

        Vector3 leftBoundary = Quaternion.Euler(0f, -interactionAngle * 0.5f, 0f) * interactionOrigin.forward;

        Vector3 rightBoundary =  Quaternion.Euler(0f, interactionAngle * 0.5f, 0f) * interactionOrigin.forward;

        Gizmos.DrawLine(interactionOrigin.position, interactionOrigin.position + leftBoundary * interactionDistance);

        Gizmos.DrawLine(interactionOrigin.position, interactionOrigin.position + rightBoundary * interactionDistance);

        Gizmos.DrawWireSphere(interactionOrigin.position, interactionDistance);
    }
}