using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class PauseMenu : BaseMenu
    {
        [SerializeField] private Button m_buttonResume;
        [SerializeField] private Button m_buttonRestart;
        [SerializeField] private Button m_buttonExitToMainMenu;

        private bool m_isPaused = false;

        void Start()
        {
            m_buttonResume.onClick.AddListener(GameResume);
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!m_isPaused)
                {
                    Show();
                }
                else
                {
                    Close();
                }
            }
        }

        public override void Show()
        {
            base.Show(); // Calls BaseMenu.Show()
            Time.timeScale = 0f; // Pause game
            m_isPaused = true;
        }

        public override void Close()
        {
            base.Close(); // Calls BaseMenu.Close()
            Time.timeScale = 1f; // Resume game
            m_isPaused = false;
        }

        private void GameResume()
        {
            Close();
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
    }
}
