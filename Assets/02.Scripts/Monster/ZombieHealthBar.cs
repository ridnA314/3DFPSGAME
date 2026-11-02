using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [Tooltip("The bar is hidden beyond this distance from the player")]
    [SerializeField] private float _visibleDistance = 15f;

    private Zombie _zombie;
    private Canvas _canvas;
    private Transform _cameraTransform;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _canvas = GetComponentInChildren<Canvas>(true);
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _zombie.OnDamaged += RefreshHealthBar;
        RefreshHealthBar();
    }

    private void LateUpdate()
    {
        // a world-space canvas costs batching every frame even when nobody looks at it
        bool visible = IsVisible();
        if (_canvas.enabled != visible)
        {
            _canvas.enabled = visible;
        }

        if (!visible) return;

        //billboard : let health bar forward to camera
        _slider.transform.forward = _cameraTransform.forward;
    }

    // show only for a damaged zombie near the player
    private bool IsVisible()
    {
        if (_zombie.Health >= _zombie.MaxHealth || _zombie.Player == null) return false;

        Vector3 toPlayer = _zombie.Player.transform.position - transform.position;
        return toPlayer.sqrMagnitude <= _visibleDistance * _visibleDistance;
    }

    private void RefreshHealthBar()
    {
        _slider.value = _zombie.Health / _zombie.MaxHealth;
    }
}