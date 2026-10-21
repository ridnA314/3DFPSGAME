using UnityEngine;
using Redcode.Pools;

public class BombPoolManager : MonoBehaviour
{
    private static BombPoolManager _instance;
    public static BombPoolManager Instance => _instance;

    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private int _poolSize = 50;
    [SerializeField] private Transform _bombContainer;

    private Pool<Bomb> _bombPool;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }

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