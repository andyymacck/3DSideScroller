using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SideScroller
{
    public class ImageFade : MonoBehaviour
    {
        [SerializeField] private Image m_fadeImage;
        [SerializeField] private Color m_colorFadeIn;
        [SerializeField] private Color m_colorFadeOut;

        private float m_fadeDuration = 1f;


        void Start()
        {

        }

        public void StartImageFade(bool isFadeout)
        {
            StartCoroutine(ScreenFadeCoroutine(isFadeout));
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
}