public class HealthChangeEvent : BaseEvent
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }

    public HealthChangeEvent(int currentHealth, int maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}