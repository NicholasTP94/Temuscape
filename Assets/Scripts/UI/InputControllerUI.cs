using UnityEngine;

public class InputControllerUI : MonoBehaviour
{
    private bool isUIOpen;

    public bool IsUIOpen => isUIOpen;

    private void Awake()
    {
        LockedCursor();
    }

    public void UnlockedCursor()
    {
        isUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockedCursor()
    {
        isUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}