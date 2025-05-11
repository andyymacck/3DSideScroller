using UnityEngine;

namespace SideScroller
{
    public class EnemyBasic : EnemyBase
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private EnemyMovement m_enemyMovement;
        [SerializeField] private EnemyState m_currentState = EnemyState.Idle;


        private enum EnemyState
        {
            Idle,
            Patrol,
            EnemyAproach,
            Attack
        }

        private void Update()
        {
            if (m_playerObject == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, m_playerObject.transform.position);

            // State transitions based on distance to player
            if (distanceToPlayer > m_idleRange)
            {
                m_currentState = EnemyState.Patrol;
            }
            else if (distanceToPlayer >= m_attackRange && distanceToPlayer <= m_idleRange)
            {
                m_currentState = EnemyState.EnemyAproach;
            }
            else if (distanceToPlayer < m_attackRange)
            {
                m_currentState = EnemyState.Attack;
            }

            // Actions based on the current state
            switch (m_currentState)
            {
                case EnemyState.Patrol:
                    m_enemyMovement.Patrol();
                    break;

                case EnemyState.EnemyAproach:
                    m_enemyMovement.MoveToObject(m_playerObject);
                    break;

                case EnemyState.Attack:
                    Attack();
                    break;
            }
        }

        private void Attack()
        {
            if (m_playerController != null)
            {
                if (Time.time >= m_lastAttackTime + m_attackDelay)
                {
                    m_lastAttackTime = Time.time;
                    Unit player = m_playerController;
                    player.DealDamage(m_damage);
                }

                m_enemyMovement.SetRotation(m_playerController.transform);
            }
        }

        public override void DealDamage(int damage)
        {
            if (!m_isAlive)
            {
                return;
            }

            m_healthCurrent -= damage;
            m_isAlive = m_healthCurrent > 0f;

            if (m_isAlive == false)
            {
                Die();
            }
        }

        public override void Die()
        {
            m_enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }

        public override void Heal(int amout)
        {
            throw new System.NotImplementedException();
        }
    }
}