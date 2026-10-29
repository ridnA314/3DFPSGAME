using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [SerializeField] private TextMeshProUGUI _gameStateText;
    [SerializeField] private TextMeshProUGUI _remainingEnemyText;

    [SerializeField] private PlayerStat _playerStat;

    [SerializeField] private Slider _playerHealthBar;
    [SerializeField] private Slider _playerStaminaBar;

    private const string ReadyText = "<color=#f0f000>준비</color>";
    private const string StartText = "<color=#00f0f0>시작!</color>";
    private const string GameOverText = "<color=#f00000>게임 오버</color>";

    private const string RemainingEnemyText = "{0} Stage : {1} / {2}";

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        RefreshPlayerHealthBar();
        RefreshPlayerStaminaBar();
    }

    public void RefreshPlayerHealthBar()
    {
        _playerHealthBar.value = _playerStat.Health / _playerStat.MaxHealth;
    }

    public void RefreshPlayerStaminaBar()
    {
        _playerStaminaBar.value = _playerStat.Stamina / _playerStat.MaxStamina;
    }

    public void SetActiveStateText(bool isActive)
    {
        _gameStateText.gameObject.SetActive(isActive);
    }

    public void SetReadyText()
    {
        _gameStateText.text = ReadyText;
    }

    public void SetStartText()
    {
        _gameStateText.text = StartText;
    }

    public void SetGameOverText()
    {
        _gameStateText.text = GameOverText;
    }

    public void RefreshEnemyCount(int count)
    {
        int stageId = 1;
        int maxCount = 1;

        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            stageId = gameManager.CurrentStageId;
        }

        ZombieSpawner zombieSpawner = ZombieSpawner.Instance;
        if (zombieSpawner != null)
        {
            maxCount = zombieSpawner.CurrentMaxZombieCount();
        }

        _remainingEnemyText.text = string.Format(RemainingEnemyText, stageId, count, maxCount);
    }
}