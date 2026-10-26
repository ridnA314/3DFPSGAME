using UnityEngine;

public class UI_MiniMap : MonoBehaviour
{
    [SerializeField] private MiniMapCamera _miniMapCamera;

    // ToDo. Press +/- button call method that resize of mini map in MiniMapCamera
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