using System.Text.RegularExpressions;
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

    private const int MinPasswordLength = 8;
    private const int MaxPasswordLength = 30;

    // 로컬/도메인은 '.'으로 시작·끝나거나 연속될 수 없고, 도메인에는 '.'이 하나 이상 있어야 한다.
    private static readonly Regex EmailRegex =
        new Regex(@"^[A-Za-z0-9_+-]+(\.[A-Za-z0-9_+-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)+\z");

    private static readonly Regex PasswordLengthRegex =
        new Regex($@"^.{{{MinPasswordLength},{MaxPasswordLength}}}\z", RegexOptions.Singleline);

    // 출력 가능한 ASCII 문자(0x21 '!' ~ 0x7E '~')만 허용한다. 공백·한글 등은 불가.
    private static readonly Regex PasswordCharsRegex = new Regex(@"^[!-~]+\z");

    // 전방 탐색(?=)으로 대문자, 소문자, 숫자, 기호(영문·숫자 외 문자)가 각각 하나 이상 있는지 확인한다.
    private static readonly Regex PasswordCompositionRegex =
        new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+\z");

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

    private Mode _mode;

    private PlayerPrefsAccountRepository _repository;

    private void Start()
    {
        ApplyMode(Mode.Login);
        _repository = new PlayerPrefsAccountRepository();
    }

    public void GotoLogin()
    {
        ApplyMode(Mode.Login);
    }

    public void GotoRegister()
    {
        ApplyMode(Mode.Register);
    }

    public void Login()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "Please enter a ID/Password";
            return;
        }

        if (_repository.CheckPassword(id, password))
        {
            SceneManager.LoadScene(GameSceneName);
            return;
        }

        _messageText.text = "Please check your id & password";
    }

    public void Register()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;
        string passwordConfirm = _passwordConfirmInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "Please enter a ID/Password";
            return;
        }

        // regular expression
        // certain pattern or rule of string
        // - validation of data
        // - search of string
        // - replace of string
        // - web crawling or parsing

        // ID 규칙: 이메일 형식 (로컬@도메인.최상위도메인)
        if (!EmailRegex.IsMatch(id))
        {
            _messageText.text = "ID must be an email";
            return;
        }

        // 비밀번호 규칙: 대문자, 소문자, 숫자, 기호를 모두 포함한 8~30 글자
        if (!PasswordLengthRegex.IsMatch(password))
        {
            _messageText.text = $"Password must be {MinPasswordLength}~{MaxPasswordLength} characters";
            return;
        }

        if (!PasswordCharsRegex.IsMatch(password))
        {
            _messageText.text = "Password can only contain letters, numbers and symbols";
            return;
        }

        if (!PasswordCompositionRegex.IsMatch(password))
        {
            _messageText.text = "Password must contain uppercase, lowercase, number and symbol";
            return;
        }

        if (password != passwordConfirm)
        {
            _messageText.text = "Incorrect password confirmation";
            return;
        }

        // ToDo. ID duplicate check
        if (_repository.Exists(id))
        {
            _messageText.text = "ID/Password already exists";
            return;
        }

        // ToDo. save register data
        _repository.Register(id, password);

        GotoLogin();
        _messageText.text = "Success Register! Please Login";
    }

    private void ApplyMode(Mode mode)
    {
        _mode = mode;
        bool isLoginMode = mode == Mode.Login;

        _passwordConfirmInputField.gameObject.SetActive(!isLoginMode);
        _loginButton.gameObject.SetActive(isLoginMode);
        _gotoRegisterButton.gameObject.SetActive(isLoginMode);
        _registerButton.gameObject.SetActive(!isLoginMode);
        _gotoLoginButton.gameObject.SetActive(!isLoginMode);

        _passwordInputField.text = string.Empty;
        _passwordConfirmInputField.text = string.Empty;
        _messageText.text = string.Empty;
    }
}