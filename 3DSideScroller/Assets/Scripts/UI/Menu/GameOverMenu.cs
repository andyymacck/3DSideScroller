using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class GameOverMenu : BaseMenu
    {
        [SerializeField] private Button m_buttonRestart;
        [SerializeField] private Button m_buttonExitToMainMenu;

        public override void Show()
        {
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);

            base.Show();
        }

        public override void Close()
        {
            m_buttonRestart.onClick.RemoveAllListeners();
            m_buttonExitToMainMenu.onClick.RemoveAllListeners();

            base.Close();
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