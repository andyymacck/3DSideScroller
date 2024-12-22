using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject m_root;
    [SerializeField] private Image m_healthBar;
    [SerializeField] private TMP_Text m_healthText;

    [Header("CameraFade")]
    [SerializeField] private Image m_fadeImage;
    [SerializeField] private Color m_colorFadeIn;
    [SerializeField] private Color m_colorFadeOut;

    private float m_fadeDuration = 1f;


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
        StartCoroutine(ScreenFadeCoroutine(eventData.IsFadeOut));
    }

    private IEnumerator ScreenFadeCoroutine(bool isFadeout)
    {
        float timer = 0f;
        Color startColor = isFadeout ? m_colorFadeOut : m_colorFadeIn;
        Color finishColor = isFadeout ? m_colorFadeIn : m_colorFadeOut;

        while (timer < m_fadeDuration) 
        {
            float lerpFactor = timer / m_fadeDuration;
            Color targetColor = Color.Lerp(startColor, finishColor, lerpFactor);
            m_fadeImage.color = targetColor;

            timer += Time.deltaTime;
            yield return null;
        }

        m_fadeImage.color = finishColor;
    }
}