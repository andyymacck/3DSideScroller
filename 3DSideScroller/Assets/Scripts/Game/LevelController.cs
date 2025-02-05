using UnityEngine;

namespace SideScroller
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private PlayerController m_playerController;
        [SerializeField] private EnemyManager m_enemyManager;
        

        private void Awake()
        {
            m_enemyManager.Initialize(m_playerController);
            m_playerController.Initialize(m_enemyManager);
        }
    }
}
