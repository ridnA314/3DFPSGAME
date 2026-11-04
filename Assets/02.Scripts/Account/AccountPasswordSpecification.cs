using System.Text.RegularExpressions;

public class AccountPasswordSpecification
{
    private const int MinPasswordLength = 8;
    private const int MaxPasswordLength = 30;

    private static readonly Regex PasswordLengthRegex =
        new Regex($@"^.{{{MinPasswordLength},{MaxPasswordLength}}}\z", RegexOptions.Singleline);

    // 출력 가능한 ASCII 문자(0x21 '!' ~ 0x7E '~')만 허용한다. 공백·한글 등은 불가.
    private static readonly Regex PasswordCharsRegex = new Regex(@"^[!-~]+\z");

    // 전방 탐색(?=)으로 대문자, 소문자, 숫자, 기호(영문·숫자 외 문자)가 각각 하나 이상 있는지 확인한다.
    private static readonly Regex PasswordCompositionRegex =
        new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+\z");

    // 마지막 IsSatisfiedBy() 호출에서 위반한 규칙의 메시지
    public string ErrorMessage { get; private set; } = string.Empty;

    public bool IsSatisfiedBy(string password)
    {
        if (!PasswordLengthRegex.IsMatch(password))
        {
            ErrorMessage = $"Password must be {MinPasswordLength}~{MaxPasswordLength} characters";
            return false;
        }

        if (!PasswordCharsRegex.IsMatch(password))
        {
            ErrorMessage = "Password can only contain letters, numbers and symbols";
            return false;
        }

        if (!PasswordCompositionRegex.IsMatch(password))
        {
            ErrorMessage = "Password must contain uppercase, lowercase, number and symbol";
            return false;
        }

        ErrorMessage = string.Empty;
        return true;
    }
}