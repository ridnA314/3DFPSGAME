using UnityEngine;

// camera of mini map that follow player and face to top-down
public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _height;
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ZoomIn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ZoomOut();
        }
    }

    private void LateUpdate()
    {
        // 1. reposition to above of player
        transform.position = _target.position + (Vector3.up * _height);

        // 2. rotation to target face direction
        transform.eulerAngles = new Vector3(90, _target.eulerAngles.y, 0);
    }

    //ToDo. Separating zoom-in and zoom-out method
    public void ZoomIn()
    {
        _camera.orthographicSize -= 1;
        if (_camera.orthographicSize < _minSize)
        {
            _camera.orthographicSize = _minSize;
        }
    }

    public void ZoomOut()
    {
        _camera.orthographicSize += 1;
        if (_camera.orthographicSize > _maxSize)
        {
            _camera.orthographicSize = _maxSize;
        }
    }
}