using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

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

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; //fix mouse cursor at center of screen
        Cursor.visible = false; //hide mouse cursor
    }

    public void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}