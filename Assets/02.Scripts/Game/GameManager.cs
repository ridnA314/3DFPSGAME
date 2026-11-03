using System.Collections;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    Pause,
    GameOver,
}

public class GameManager : SingletoneBehaviour<GameManager>
{
    [SerializeField] private float _startTime;
    public float StartTime => _startTime;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _optionPopup;

    private GameState _state;
    public GameState State => _state;

    private int _currentStageId;
    public int CurrentStageId => _currentStageId;

    private int _enemyCount;
    public int EnemyCount => _enemyCount;

    protected override void Awake()
    {
        base.Awake();
        _enemyCount = 0;
    }

    private void Start()
    {
        _optionPopup.SetActive(false);
        _state = GameState.Ready;
        _currentStageId = 1;

        StartCoroutine(StartSequence_Coroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_state == GameState.Playing)
            {
                _state = GameState.Pause;
                Time.timeScale = 0f;
                _optionPopup.SetActive(true);
                CursorManager.Instance?.UnLockCursor();
            }
            else if (_state == GameState.Pause)
            {
                _state = GameState.Playing;
                Time.timeScale = 1f;
                _optionPopup.SetActive(false);
                CursorManager.Instance?.LockCursor();
            }
        }
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