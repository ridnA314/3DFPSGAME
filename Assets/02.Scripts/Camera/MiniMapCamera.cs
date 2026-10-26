using UnityEngine;

// camera of mini map that follow player and face to top-down
public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _height;

    private void LateUpdate()
    {
        // 1. reposition to above of player
        transform.position = _target.position + (Vector3.up * _height);

        // 2. rotation to target face direction
        transform.eulerAngles = new Vector3(90, _target.eulerAngles.y, 0);
    }
}