using System.Text.RegularExpressions;

public class AccountIdSpecification
{
    // 로컬/도메인은 '.'으로 시작·끝나거나 연속될 수 없고, 도메인에는 '.'이 하나 이상 있어야 한다.
    private static readonly Regex EmailRegex =
        new Regex(@"^[A-Za-z0-9_+-]+(\.[A-Za-z0-9_+-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)+\z");

    public string ErrorMessage => "ID must be an email form";

    public bool IsSatisfiedBy(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}