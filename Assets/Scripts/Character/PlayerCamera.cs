using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference cameraLookAction;
    [SerializeField] private InputActionReference cameraZoomAction;


    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float zoomSmoothSpeed = 8f;
    [SerializeField] private float followSmoothSpeed = 10f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private float collisionOffset = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundClearance = 0.3f;

    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;

    [SerializeField] private InputControllerUI inputControllerUI;


    private float yaw;
    private float pitch;
    private float targetDistance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 currentRotation = transform.eulerAngles;

        yaw = currentRotation.y;
        pitch = currentRotation.x;

        targetDistance = distance;
    }

    private void LateUpdate()
    {
        if (target == null || cameraLookAction == null)
        {
            return;
        }

        Vector2 lookInput = Vector2.zero;
        Vector2 zoomInput = Vector2.zero;

        if (inputControllerUI == null || !inputControllerUI.IsUIOpen)
        {
            lookInput = cameraLookAction.action.ReadValue<Vector2>();

            zoomInput = cameraZoomAction.action.ReadValue<Vector2>();
        }

        targetDistance -= zoomInput.y * zoomSpeed * 0.01f;

        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        distance = Mathf.Lerp(distance, targetDistance, zoomSmoothSpeed * Time.deltaTime);

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position + Vector3.up * height;

        Vector3 cameraOffset = rotation * new Vector3(0f, 0f, -distance);

        Vector3 desiredPosition = targetPosition + cameraOffset;

        Vector3 directionToCamera = desiredPosition - targetPosition;

        float desiredDistance = directionToCamera.magnitude;

        if (desiredDistance > 0.001f)
        {
            if (Physics.SphereCast(targetPosition, collisionRadius, directionToCamera,
            out RaycastHit hit, desiredDistance, collisionMask, QueryTriggerInteraction.Ignore))
            {
                desiredPosition = targetPosition + directionToCamera * Mathf.Max(hit.distance - collisionOffset, 0f);
            }
        }

        Vector3 finalPosition = Vector3.Lerp(transform.position, desiredPosition, followSmoothSpeed * Time.deltaTime);

        Vector3 groundCheckStart = finalPosition + Vector3.up * 5f;

        if (Physics.Raycast(groundCheckStart, Vector3.down,
        out RaycastHit groundHit,10f, groundMask, QueryTriggerInteraction.Ignore))
        {
            float minimumCameraHeight = groundHit.point.y + groundClearance;

            if (finalPosition.y < minimumCameraHeight)
            {
                finalPosition.y = minimumCameraHeight;
            }
        }

        transform.position = finalPosition;

        transform.LookAt(targetPosition);
    }
}
