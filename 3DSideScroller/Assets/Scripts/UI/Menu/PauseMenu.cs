using LevelManagerLoader;
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

        public override void Show()
        {
            PauseGame();
            base.Show();
        }

        public override void Close()
        {
            GameResume();

            base.Close();
        }

        private void GameResume()
        {
            m_buttonResume.onClick.RemoveAllListeners();
            m_buttonRestart.onClick.RemoveAllListeners();
            m_buttonExitToMainMenu.onClick.RemoveAllListeners();

            Time.timeScale = 1f;
        }

        private void GameRestart()
        {
            LevelManager.Restart();
            Time.timeScale = 1f;
        }

        private void GameExitToMenu()
        {
            LevelManager.LoadLevelByNum(LevelGroupType.Menu, 1);
            Time.timeScale = 1f;
        }

        private void PauseGame()
        {
            m_buttonResume.onClick.AddListener(GameResume);
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);

            Time.timeScale = 0f;
        }
    }
}