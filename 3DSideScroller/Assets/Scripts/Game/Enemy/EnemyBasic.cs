using UnityEngine;

namespace SideScroller
{
    public class EnemyBasic : Unit
    {
        [Header("Attack param")]
        [SerializeField] private float m_attackRange = 1f;
        [SerializeField] private float m_idleRange = 5f;
        [SerializeField] private float m_attackDelay = 1f;
        [SerializeField] private float m_damage = 1f;
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private EnemyMovement m_enemyMovement;

        private GameObject m_playerObject;
        private Unit m_playerController;
        private float m_lastAttackTime = 0f;


        private void Start()
        {
            SetPlayer(GameObject.FindGameObjectWithTag(Constants.PlayerTagId));
        }

        private void Update()
        {
            if (m_playerObject != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, m_playerObject.transform.position);

                // update idle and partrol
                // add enemy state kind of (idle, patroll, attack)
                if (distanceToPlayer > m_idleRange)
                {
                    m_enemyMovement.Patrol();
                    return;
                }

                if (distanceToPlayer >= m_attackRange)
                {
                    m_enemyMovement.MoveToObject(m_playerObject);
                }

                if (distanceToPlayer < m_attackRange)
                {
                    Attack();
                }
            }
        }

        public void SetPlayer(GameObject playerObject)
        {
            m_playerObject = playerObject;
            m_playerController = playerObject.GetComponent<PlayerController>();
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
            }
        }

        public override void DealDamage(float damage)
        {
            if (!m_isAlive)
            {
                return;
            }

            m_health -= damage;
            m_isAlive = m_health > 0f;

            if (m_isAlive == false)
            {
                Die();
            }
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}