using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Zombie _zombie;
    private Transform _cameraTransform;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _zombie.OnDamaged += RefreshHealthBar;
        RefreshHealthBar();
    }

    private void LateUpdate()
    {
        //billboard : let health bar forward to camera
        _slider.transform.forward = _cameraTransform.forward;
    }

    private void RefreshHealthBar()
    {
        _slider.value = _zombie.Health / _zombie.MaxHealth;
    }
}
