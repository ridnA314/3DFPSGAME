using UnityEngine;

public class UI_MiniMap : MonoBehaviour
{
    [SerializeField] private MiniMapCamera _miniMapCamera;

    public void ClickZoomIn()
    {
        if (_miniMapCamera == null) return;
        _miniMapCamera.ZoomIn();
    }

    public void ClickZoomOut()
    {
        if (_miniMapCamera == null) return;
        _miniMapCamera.ZoomOut();
    }
}