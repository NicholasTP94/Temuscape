using TMPro;
using UnityEngine;

public class GatherFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float displayTime = 1.2f;

    private Transform currentTarget;
    private Collider currentTargetCollider;
    private float timer;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        feedbackText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!feedbackText.gameObject.activeSelf)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Hide();
        }
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
        {
            return;
        }

        Vector3 position;

        if (currentTargetCollider != null)
        {
            position = currentTargetCollider.bounds.center;
        }
        else
        {
            position = currentTarget.position;
        }

        transform.position = position;

        Vector3 direction = playerCamera.transform.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void ShowWrongTool(Transform target)
    {
        if (target == null)
        {
            return;
        }

        currentTarget = target;
        currentTargetCollider = target.GetComponentInChildren<Collider>();

        feedbackText.text = "Wrong Tool";
        feedbackText.gameObject.SetActive(true);

        timer = displayTime;
    }

    private void Hide()
    {
        currentTarget = null;
        currentTargetCollider = null;

        feedbackText.gameObject.SetActive(false);
    }
}