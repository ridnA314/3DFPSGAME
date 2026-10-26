using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void Start()
    {
        _zombie.OnDamaged += RefreshHealthBar;
    }

    private void LateUpdate()
    {
        //billboard : let health bar forward to camera
        _slider.transform.forward = Camera.main.transform.forward;
    }

    private void RefreshHealthBar()
    {
        _slider.value = _zombie.Health / _zombie.MaxHealth;
    }
}