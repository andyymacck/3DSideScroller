using UnityEngine;

namespace SideScroller
{
    public abstract class EnemyBase : Unit
    {
        [Header("Attack parameters")]
        [SerializeField] protected float m_idleRange = 12f; // Distance to stop chasing
        [SerializeField] protected float m_damage = 5f; // Damage per laser

        protected GameObject m_playerObject;
        protected Unit m_playerController;

        public void SetPlayer(PlayerController player)
        {
            m_playerObject = player.gameObject;
            m_playerController = player;
        }
    }
}