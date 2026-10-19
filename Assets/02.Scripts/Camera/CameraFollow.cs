using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Awake()
    {
        if (_target == null)
        {
            Debug.LogWarning("Can not found [CameraFollow] target!");
        }
    }

    private void LateUpdate()
    {
        transform.position = _target.position;
    }
}