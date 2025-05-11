using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SideScroller
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_root;
        [SerializeField] private ImageFade m_imageFade;
        [SerializeField] private GameOverMenu m_gameOverMenu;
        [SerializeField] private PauseMenu m_pauseMenu;
        [SerializeField] private FinishMenu m_finishMenu;

        private bool m_isPause = false;


        private void Awake()
        {
            EventHub.Instance.Subscribe<ScreenFadeEvent>(ScreenFade);
            EventHub.Instance.Subscribe<GameOverEvent>(OnGameOver);
            EventHub.Instance.Subscribe<LevelFinishedEvent>(OnLevelFinished);

            m_root.SetActive(true);
            m_gameOverMenu.Close();
            m_pauseMenu.Close();
            m_finishMenu.Close();
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<ScreenFadeEvent>(ScreenFade);

            EventHub.Instance.UnSubscribe<GameOverEvent>(OnGameOver);
            EventHub.Instance.UnSubscribe<LevelFinishedEvent>(OnLevelFinished);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!m_isPause)
                {
                    m_isPause = true;
                    m_pauseMenu.Show();
                }
                else
                {
                    m_isPause = false;
                    m_pauseMenu.Close();
                }
            }
        }

        private void OnGameOver(GameOverEvent eventData)
        {
            m_imageFade.StartImageFade(true);
            m_gameOverMenu.Show();
        }

        private void OnLevelFinished(LevelFinishedEvent eventData)
        {
           m_finishMenu.Show();
        }


        private void ScreenFade(ScreenFadeEvent eventData)
        {
            m_imageFade.StartImageFade(eventData.IsFadeOut);
        }
    }
}