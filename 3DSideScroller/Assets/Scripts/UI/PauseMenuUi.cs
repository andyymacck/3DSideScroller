using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class PauseMenuUi : MonoBehaviour
    {
        [SerializeField] private GameObject m_root;
        [SerializeField] private Button m_buttonResume;
        [SerializeField] private Button m_buttonRestart;
        [SerializeField] private Button m_buttonExitToMainMenu;

        private bool m_isPause = false;

        void Start()
        {
            m_buttonResume.onClick.AddListener(GameResume);
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);
        }

        private void OnValidate()
        {
            if (m_buttonResume == null)
            {
                Debug.LogError($"{nameof(m_buttonResume)} is NULL");
            }

            if (m_buttonExitToMainMenu == null)
            {
                Debug.LogError($"{nameof(m_buttonExitToMainMenu)} is NULL");
            }

            if (m_buttonRestart == null)
            {
                Debug.LogError($"{nameof(m_buttonRestart)} is NULL");
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!m_isPause)
                {
                    PauseGame();
                }
                else
                {
                    GameResume();
                }
            }
        }

        private void GameResume()
        {
            m_isPause = false;
            Time.timeScale = 1f;
            m_root.SetActive(false);
        }

        private void GameRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }

        private void GameExitToMenu()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }

        private void PauseGame()
        {
            m_isPause = true;
            Time.timeScale = 0f;
            m_root.SetActive(true);
        }
    }
}