using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement: MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float fallGravityMultiplier = 1.5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference runAction;
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterModel;

    [SerializeField] private Transform cameraTransform;

    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isRunning;
    private bool isGrounded;

    public Vector2 MoveInput => moveInput;
    public bool IsRunning => isRunning;
    public bool ΙsGrounded => isGrounded;
    public float VerticalVelocity => verticalVelocity;

    public event Action Jumped;

    private void Awake()
    {
        //Get CharacterController
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        //Movement
        moveInput = moveAction.action.ReadValue<Vector2>();
        isRunning = runAction.action.IsPressed();

        float currentSpeed;

        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirections = cameraForward * moveInput.y + cameraRight * moveInput.x;

        //Rotate player
        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (moveDirections.sqrMagnitude > 0.01f)
        {
            Quaternion modelRotation = Quaternion.LookRotation(moveDirections);

            characterModel.rotation = Quaternion.Slerp(characterModel.rotation, modelRotation, rotationSpeed * Time.deltaTime);
        }

        // Fix VerticalVelocity
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (isGrounded && jumpAction.action.WasPressedThisFrame())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            Jumped?.Invoke();
        }

        // DownForce
        if (verticalVelocity <0f)
        {
            verticalVelocity += gravity * fallGravityMultiplier * Time.deltaTime;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Implementation of Downforce (VericvalMovement)
        Vector3 vertivalMovement = new Vector3(0f, verticalVelocity, 0f);

        //Movement Implementation
        Vector3 finalMovement = moveDirections * currentSpeed + vertivalMovement;

        //Implementation of final movement
        characterController.Move(finalMovement * Time.deltaTime);
    }
}
