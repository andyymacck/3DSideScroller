using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Image m_healthBar;
    [SerializeField] private TMP_Text m_healthText;

    private void Awake()
    {
        EventHub.Instance.Subcribe<HealthChangeEvent>(UpdateHealth);
    }

    private void OnDestroy()
    {
        EventHub.Instance.UnSubcribe<HealthChangeEvent>(UpdateHealth);
    }

    private void UpdateHealth(HealthChangeEvent eventData)
    {
        float healthFactor = eventData.CurrentHealth / eventData.MaxHealth;
        m_healthBar.fillAmount = healthFactor;
        m_healthText.text = $"{eventData.CurrentHealth} / {eventData.MaxHealth}";
    }
}