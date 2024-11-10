using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller
{
    public class MainMenuUi : MonoBehaviour
    {
        [SerializeField] private Button m_buttonStart;
        [SerializeField] private Button m_buttonExit;

        void Start()
        {
            m_buttonStart.onClick.AddListener(GameStart);
            m_buttonExit.onClick.AddListener(GameExit);
        }

        private void OnValidate()
        {
            if (m_buttonStart == null)
            {
                Debug.LogError("m_buttonStart is NULL");
            }

            if (m_buttonExit == null)
            {
                Debug.LogError("m_buttonExit is NULL");
            }
        }



        void Update()
        {



        }

        private void GameStart()
        {
            SceneManager.LoadScene(1);
        }

        private void GameExit()
        {
            Application.Quit();
        }


    }
}