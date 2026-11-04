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

        // ToDo. set rule about id and password
        // ID is email form
        // Password that its range of length is 7~15
        // must be english and contains special letter and upper case letter less than one each

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "Please enter a ID/Password";
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