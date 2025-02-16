using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class GameOverMenu : MonoBehaviour //try to do that all menu classes inhrit from a basic menu class with show and close functionality
    {
        [SerializeField] private Button m_buttonRestart;
        [SerializeField] private Button m_buttonExitToMainMenu;

        // Start is called before the first frame update
        void Start()
        {
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show() //inherit from a base class and then override
        {
            this.gameObject.SetActive(true);
            m_buttonRestart.onClick.AddListener(GameRestart);
            m_buttonExitToMainMenu.onClick.AddListener(GameExitToMenu);
            // bla bla
        }

        public void Close() //inherit from a base class and then override
        {
            this.gameObject.SetActive(false);
            m_buttonRestart.onClick.RemoveAllListeners();
            m_buttonExitToMainMenu.onClick.RemoveAllListeners();
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
