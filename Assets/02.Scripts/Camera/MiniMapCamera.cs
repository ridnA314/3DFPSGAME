using UnityEngine;

// camera of mini map that follow player and face to top-down
public class MiniMapCamera : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _height;

    [Header("Zoom")]
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _zoomStep = 1f;
    [SerializeField] private KeyCode _zoomInKey = KeyCode.Alpha0;
    [SerializeField] private KeyCode _zoomOutKey = KeyCode.Alpha9;

    private const float TopDownPitch = 90f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (_target == null)
        {
            GameLogger.LogError($"{nameof(MiniMapCamera)}: target is not assigned.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_zoomInKey))
        {
            ZoomIn();
        }
        else if (Input.GetKeyDown(_zoomOutKey))
        {
            ZoomOut();
        }
    }

    private void LateUpdate()
    {
        // 1. reposition to above of player
        // 2. rotation to target face direction
        transform.SetPositionAndRotation(
            _target.position + (Vector3.up * _height),
            Quaternion.Euler(TopDownPitch, _target.eulerAngles.y, 0f));
    }

    public void ZoomIn()
    {
        ChangeZoom(-_zoomStep);
    }

    public void ZoomOut()
    {
        ChangeZoom(_zoomStep);
    }

    private void ChangeZoom(float delta)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + delta, _minSize, _maxSize);
    }
}
