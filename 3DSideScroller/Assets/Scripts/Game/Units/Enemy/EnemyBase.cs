using UnityEngine;

namespace SideScroller
{
    public abstract class EnemyBase : Unit
    {
        [Header("Attack parameters")]
        [SerializeField] protected float m_idleRange = 12f; // Distance to stop chasing
        [SerializeField] protected int m_damage = 1; // Damage per laser

        protected EnemyManager m_enemyManager;
        protected GameObject m_playerObject;
        protected Unit m_playerController;

        public void SetPlayer(PlayerController player)
        {
            m_playerObject = player.gameObject;
            m_playerController = player;
        }

        public void DisableEnemy()
        {
            //Need to set enemy unactive, not die
            Die();
        }

        public void SetEnemyManager(EnemyManager manager)
        {
            m_enemyManager = manager;
        }
    }
}