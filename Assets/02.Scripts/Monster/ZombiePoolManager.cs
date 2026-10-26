using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ZombiePoolManager : MonoBehaviour
{
    private static ZombiePoolManager _instance;
    public static ZombiePoolManager Instance => _instance;

    [SerializeField] private List<ZombieEntry> _zombieEntries;
    private Dictionary<ZombieType, ObjectPool<Zombie>> _zombiePools;
    [SerializeField] private Transform _zombieContainer;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _zombiePools = new Dictionary<ZombieType, ObjectPool<Zombie>>();
        foreach (ZombieEntry zombieEntry in _zombieEntries)
        {
            _zombiePools[zombieEntry.Type] =
                new ObjectPool<Zombie>(
                    () =>
                    {
                        Zombie zombie = Instantiate(zombieEntry.ZombiePrefab, _zombieContainer);
                        return zombie;
                    },
                    ActivateProjectile,
                    DisableProjectile,
                    DestroyProjectile,
                    false,
                    zombieEntry.InitialAmount,
                    zombieEntry.MaxAmount);
        }
    }

    public Zombie Get(ZombieType type)
    {
        return _zombiePools[type].Get();
    }

    public void Release(Zombie zombie)
    {
        _zombiePools[zombie.Type].Release(zombie);
    }

    private void ActivateProjectile(Zombie zombie)
    {
        zombie.gameObject.SetActive(true);
    }

    private void DisableProjectile(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private void DestroyProjectile(Zombie zombie)
    {
        if (zombie == null) return;
        Destroy(zombie.gameObject);
    }
}