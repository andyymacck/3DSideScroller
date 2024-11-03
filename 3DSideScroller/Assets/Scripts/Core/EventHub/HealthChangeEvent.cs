public class HealthChangeEvent : BaseEvent
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public HealthChangeEvent(float currentHealth, float maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}