using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private static ZombieSpawner _instance;
    public static ZombieSpawner Instance => _instance;

    [SerializeField] private List<ZombieSpawnTable> _zombieSpawnTables;
    private Dictionary<int, ZombieSpawnTable> _spawnTables;

    [SerializeField] private Bounds _spawnBounds;
    [SerializeField] private LayerMask _evadeTargetMask;
    [SerializeField] private float _searchRadius;

    private Coroutine _spawnCoroutine;
    private int _maxExtractCount = 50;

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
        _spawnTables = new Dictionary<int, ZombieSpawnTable>();
        foreach (ZombieSpawnTable table in _zombieSpawnTables)
        {
            _spawnTables[table.StageID] = table;
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = _spawnBounds.min.x;
        float randomZ = _spawnBounds.min.z;
        Vector3 randomPos = new Vector3(randomX, 1, randomZ);
        int repeat = 0;
        while (repeat < _maxExtractCount)
        {
            randomX = Random.Range(_spawnBounds.min.x, _spawnBounds.max.x);
            randomZ = Random.Range(_spawnBounds.min.z, _spawnBounds.max.z);
            randomPos.x = randomX;
            randomPos.z = randomZ;

            Collider[] colliders = Physics.OverlapSphere(
                randomPos,
                _searchRadius,
                _evadeTargetMask);

            if (colliders.Length <= 0)
            {
                break;
            }

            repeat++;
        }

        return randomPos;
    }

    public void StartSpawn()
    {
        int stageID = 1;
        GameManager manager = GameManager.Instance;
        if (manager != null)
        {
            stageID = manager.CurrentStageId;
        }

        if (_spawnTables.TryGetValue(stageID, out ZombieSpawnTable table))
        {
            Debug.Log(stageID + " spawned");
            _spawnCoroutine = StartCoroutine(Spawn_Coroutine(table));
        }
    }

    public void StopSpawn()
    {
        StopCoroutine(_spawnCoroutine);
        _spawnTables.Clear();
    }

    public int CurrentMaxZombieCount()
    {
        GameManager manager = GameManager.Instance;
        if (manager == null) return 0;

        int stageId = manager.CurrentStageId;
        int count = 0;
        foreach (ZombieSpawnTable table in _spawnTables.Values)
        {
            if (table.StageID == stageId)
            {
                foreach (ZombieSpawnData data in table.ZombieSpawnDataList)
                {
                    count += data.SpawnCount;
                }
            }
        }

        return count;
    }

    private IEnumerator Spawn_Coroutine(ZombieSpawnTable table)
    {
        int spawnCount;
        Zombie zombie;
        foreach (ZombieSpawnData data in table.ZombieSpawnDataList)
        {
            spawnCount = 0;
            while (spawnCount < data.SpawnCount)
            {
                yield return new WaitForSeconds(data.SpawnInterval);

                zombie = ZombiePoolManager.Instance?.Get(data.Type);
                if (zombie != null)
                {
                    zombie.transform.position = GetRandomPosition();
                }

                spawnCount++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_spawnBounds.center, _spawnBounds.size);
    }
}