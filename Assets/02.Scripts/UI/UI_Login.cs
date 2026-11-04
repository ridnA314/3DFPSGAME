using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 하나의 패널에서 로그인/회원가입 모드를 전환하며 필요한 요소만 보여준다.
public class UI_Login : MonoBehaviour
{
    private enum Mode
    {
        Login,
        Register,
    }

    private const string GameSceneName = "GameScene";

    [Header("ID/Password Input Fields")]
    [SerializeField] private TMP_InputField _idInputField;

    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;

    [Header("Buttons")]
    [SerializeField] private Button _loginButton;

    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _gotoLoginButton;

    [Header("Etc")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private Mode _mode = Mode.Login;

    private void Start()
    {
        ApplyMode(Mode.Login);
    }

    public void Login()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "아이디와 비밀번호를 입력해 주세요.";
            return;
        }

        SceneManager.LoadScene(GameSceneName);
    }

    public void Register()
    {
    }

    private void ApplyMode(Mode mode)
    {
        _mode = mode;
        bool isLogin = mode == Mode.Login;

        _passwordConfirmInputField.gameObject.SetActive(!isLogin);
        _loginButton.gameObject.SetActive(isLogin);
        _gotoRegisterButton.gameObject.SetActive(isLogin);
        _registerButton.gameObject.SetActive(!isLogin);
        _gotoLoginButton.gameObject.SetActive(!isLogin);

        _passwordInputField.text = string.Empty;
        _passwordConfirmInputField.text = string.Empty;
        _messageText.text = string.Empty;
    }
}