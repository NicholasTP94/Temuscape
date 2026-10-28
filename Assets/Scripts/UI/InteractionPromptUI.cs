using UnityEngine;
using TMPro;

public class InteractionPromptUI: MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private Camera playerCamera;

    private Transform currentTarget;

    private Collider currentTargetCollider;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        promptText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
        {
            return;
        }

        Vector3 promptPosition;

        if (currentTargetCollider != null)
        {
            promptPosition = currentTargetCollider.bounds.center;
        }
        else
        {
            promptPosition = currentTarget.position;
        }

        transform.position = promptPosition;

        Vector3 direction = playerCamera.transform.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void ShowPrompt(Transform target, string InteractionText)
    {
        if (target == null)
        {
            return;
        }

        currentTarget = target;

        currentTargetCollider = currentTarget.GetComponent<Collider>();

        promptText.text = $"[E] {InteractionText}";

        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        currentTarget = null;

        currentTargetCollider = null;

        promptText.gameObject.SetActive (false);
    }
}