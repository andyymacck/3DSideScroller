using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SideScroller
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_root;
        [SerializeField] private Image m_healthBar;
        [SerializeField] private TMP_Text m_healthText;
        [SerializeField] private ImageFade m_imageFade;

        
        private void Awake()
        {
            EventHub.Instance.Subscribe<HealthChangeEvent>(UpdateHealth);
            EventHub.Instance.Subscribe<ScreenFadeEvent>(ScreenFade);
            m_root.SetActive(true);
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<HealthChangeEvent>(UpdateHealth);
            EventHub.Instance.UnSubscribe<ScreenFadeEvent>(ScreenFade);
        }

        private void UpdateHealth(HealthChangeEvent eventData)
        {
            float healthFactor = eventData.CurrentHealth / eventData.MaxHealth;
            m_healthBar.fillAmount = healthFactor;
            m_healthText.text = $"{eventData.CurrentHealth} / {eventData.MaxHealth}";
        }

        private void ScreenFade(ScreenFadeEvent eventData)
        {
            m_imageFade.StartImageFade(eventData.IsFadeOut);
        }
    }
}