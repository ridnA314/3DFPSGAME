using System.Collections;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private float _startTime;
    [SerializeField] private Animator _playerAnimator;

    private GameState _state;
    public GameState State => _state;

    private int _currentStageId;
    public int CurrentStageId => _currentStageId;

    private int _enemyCount;
    public int EnemyCount => _enemyCount;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _enemyCount = 0;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _state = GameState.Ready;
        _currentStageId = 1;

        StartCoroutine(StartSequence_Coroutine());
    }

    public bool IsPlaying()
    {
        return _state == GameState.Playing;
    }

    private IEnumerator StartSequence_Coroutine()
    {
        UIManager.Instance?.SetActiveStateText(true);
        UIManager.Instance?.SetReadyText();

        yield return new WaitForSeconds(_startTime);

        UIManager.Instance?.SetStartText();
        _state = GameState.Playing;

        yield return new WaitForSeconds(1f);

        UIManager.Instance?.SetActiveStateText(false);
        ZombieSpawner.Instance?.StartSpawn();
    }

    public void GameOver()
    {
        _state = GameState.GameOver;
        UIManager.Instance?.SetActiveStateText(true);
        UIManager.Instance?.SetGameOverText();

        _playerAnimator.SetFloat("MoveMotion", 0f);
    }

    public void IncreaseEnemy()
    {
        _enemyCount += 1;
        UIManager.Instance?.RefreshEnemyCount(_enemyCount);
    }

    public void DecreaseEnemy()
    {
        _enemyCount -= 1;
        UIManager.Instance?.RefreshEnemyCount(_enemyCount);
    }
}