using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject _gameoverPopup;

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
        _gameoverPopup.SetActive(false);
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
        _gameoverPopup.SetActive(true);
        CursorManager.Instance?.UnLockCursor();
        //UIManager.Instance?.SetGameOverText();

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

    public void Resume()
    {
        _state = GameState.Playing;
        Time.timeScale = 1f;
        _optionPopup.SetActive(false);
        CursorManager.Instance?.LockCursor();
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        // 유지 중인 매니저들은 이전 씬의 UI를 참조하므로 현재 씬과 함께 언로드하고, 새 씬의 매니저로 교체한다.
        PersistentObjects.ReturnToActiveScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}