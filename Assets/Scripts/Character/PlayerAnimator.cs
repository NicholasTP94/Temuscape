using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerGathering playerGathering;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (playerGathering == null)
        {
            playerGathering = GetComponentInParent<PlayerGathering>();
        }
    }
    private void OnEnable()
    {
        // Jump event listener on
        playerMovement.Jumped += HandleJumped;
    }
    private void OnDisable()
    {
        // Jump event listener off
        playerMovement.Jumped -= HandleJumped;
    }

    private void Update()
    {
        // Check if player is moving
        bool isMoving = playerMovement.MoveInput.sqrMagnitude > 0.01f;

        //Default animation speed while standing still
        float animationSpeed = 0f;

        // Animation speed for Running and walking
        if (isMoving)
        {
            animationSpeed = playerMovement.IsRunning ? 1f : 0.5f;
        }

        // Sends movement speed to the Animator
        animator.SetFloat("Speed", animationSpeed);

        // Ground Check for animator
        animator.SetBool("ΙsGrounded", playerMovement.ΙsGrounded);

        //Sends vertical movement speed for Jumping to the Animator
        animator.SetFloat("VerticalVelocity", playerMovement.VerticalVelocity);
    }

    // Starts the jump animation
    private void HandleJumped()
    {
        animator.SetTrigger("Jump");
    }

    // Starts the mining animation
    public void PlayMining()
    {
        animator.SetTrigger("Mining");
    }

    // Starts the Interaction animation
    public void SetInteraction(bool isInteracting)
    {
        animator.SetBool("Interaction", isInteracting);
    }

    // Called as an animation event when the tool hits the resource
    public void AnimationGatherHit()
    {
        playerGathering.PerformGatherHit();
    }

    // Called as an animation event when the tool/ mining animation ends
    public void AnimationGatherFinished()
    {
        playerGathering.FinishGathering();
    }
}
