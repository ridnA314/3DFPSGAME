[System.Serializable]
public class PlayerStatus
{
    public float Stamina { get; set; }
    public float MaxStamina;

    public float Health { get; set; }
    public float MaxHealth;

    public void Init()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
    }
}