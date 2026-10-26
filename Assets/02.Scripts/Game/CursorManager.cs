using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        LockCursor();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            UnLockCursor();
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; //fix mouse cursor at center of screen
        Cursor.visible = false; //hide mouse cursor
    }

    private void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}