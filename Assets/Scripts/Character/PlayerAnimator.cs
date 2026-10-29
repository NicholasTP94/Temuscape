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
        playerMovement.Jumped += HandleJumped;
    }
    private void OnDisable()
    {
        playerMovement.Jumped -= HandleJumped;
    }

    private void Update()
    {
        bool isMoving = playerMovement.MoveInput.sqrMagnitude > 0.01f;

        float animationSpeed = 0f;

        if (isMoving)
        {
            animationSpeed = playerMovement.IsRunning ? 1f : 0.5f;
        }

        animator.SetFloat("Speed", animationSpeed);

        animator.SetBool("ΙsGrounded", playerMovement.ΙsGrounded);

        animator.SetFloat("VerticalVelocity", playerMovement.VerticalVelocity);
    }
    private void HandleJumped()
    {
        animator.SetTrigger("Jump");
    }

    public void PlayMining()
    {
        animator.SetTrigger("Mining");
    }

    public void SetInteraction(bool isInteracting)
    {
        animator.SetBool("Interaction", isInteracting);
    }

    public void AnimationGatherHit()
    {
        playerGathering.PerformGatherHit();
    }

    public void AnimationGatherFinished()
    {
        playerGathering.FinishGathering();
    }
}
