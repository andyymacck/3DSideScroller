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

            EventHub.Instance.Subscribe<LevelFinishedEvent>(OnLevelFinish);
        }

        private void OnDestroy()
        {
            EventHub.Instance.Subscribe<LevelFinishedEvent>(OnLevelFinish);
        }

        private void OnLevelFinish(LevelFinishedEvent eventData)
        {
            m_enemyManager.DisableEnemies();
        }
    }
}
