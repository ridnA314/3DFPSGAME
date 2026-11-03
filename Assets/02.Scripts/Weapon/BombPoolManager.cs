using UnityEngine;
using Redcode.Pools;

public class BombPoolManager : SingletoneBehaviour<BombPoolManager>
{
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private int _poolSize = 50;
    [SerializeField] private Transform _bombContainer;

    private Pool<Bomb> _bombPool;

    private void Start()
    {
        _bombPool = Pool.Create(_bombPrefab, _poolSize, _bombContainer);
    }

    public Bomb Get()
    {
        return _bombPool.Get();
    }

    public void Release(Bomb bomb)
    {
        _bombPool.Take(bomb);
    }
}