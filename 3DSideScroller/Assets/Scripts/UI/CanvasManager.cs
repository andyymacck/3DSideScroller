using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject m_root;
    [SerializeField] private Image m_healthBar;
    [SerializeField] private TMP_Text m_healthText;

    private void Awake()
    {
        EventHub.Instance.Subscribe<HealthChangeEvent>(UpdateHealth);
        m_root.SetActive(true);
    }

    private void OnDestroy()
    {
        EventHub.Instance.UnSubscribe<HealthChangeEvent>(UpdateHealth);
    }

    private void UpdateHealth(HealthChangeEvent eventData)
    {
        float healthFactor = eventData.CurrentHealth / eventData.MaxHealth;
        m_healthBar.fillAmount = healthFactor;
        m_healthText.text = $"{eventData.CurrentHealth} / {eventData.MaxHealth}";
    }
}