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

    private GameState _state;
    public GameState State => _state;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _state = GameState.Ready;
        StartCoroutine(StartSequence_Coroutine());
    }

    public bool IsPlaying()
    {
        return _state == GameState.Playing;
    }

    private IEnumerator StartSequence_Coroutine()
    {
        yield return new WaitForSeconds(_startTime);
        _state = GameState.Playing;
    }
}