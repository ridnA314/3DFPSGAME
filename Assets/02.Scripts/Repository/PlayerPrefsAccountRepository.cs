using UnityEngine;

public class PlayerPrefsAccountRepository
{
    // account data repository
    private const string KeyPrefix = "Account_";

    private string GetKeyID(string id)
    {
        return KeyPrefix + id;
    }

    // Register
    public void Register(string id, string password)
    {
        PlayerPrefs.SetString(GetKeyID(id), password);
        PlayerPrefs.Save();
    }

    // ID duplicate check
    public bool Exists(string id)
    {
        return PlayerPrefs.HasKey(GetKeyID(id));
    }

    // password validation
    public bool CheckPassword(string id, string password)
    {
        return Exists(id) && PlayerPrefs.GetString(GetKeyID(id)) == password;
    }
}