using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class GameOverMenu : BaseMenu
    {
        [SerializeField] private Button m_buttonRestart;
        [SerializeField] private Button m_buttonExitToMainMenu;

        private void Start()
        {
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);
        }

        public override void Show()
        {
            base.Show(); // Calls BaseMenu.Show()
        }

        public override void Close()
        {
            base.Close(); // Calls BaseMenu.Close()
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

