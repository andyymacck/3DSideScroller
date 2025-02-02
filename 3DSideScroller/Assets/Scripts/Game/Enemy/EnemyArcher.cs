using UnityEngine;

namespace SideScroller
{
    public class EnemyArcher : Unit
    {
        [SerializeField] private float m_moveSpeed = 2f;
        [Header("Attack param")]
        [SerializeField] private float m_idleRange = 5f;
        [SerializeField] private float m_damage = 1f;
        [SerializeField] private float m_speed = 2f;
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private Bullet m_bulletPrefab;


        private GameObject m_playerObject;
        private Unit m_playerController;
        


        private void Start()
        {
            SetPlayer(GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_ID));
        }


        private void Update()
        {
            if (m_playerObject != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, m_playerObject.transform.position);

                if (distanceToPlayer > m_idleRange)
                {
                    return;
                }

                if (distanceToPlayer >= m_attackRange)
                {
                    //MoveToPlayer(m_playerObject);
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

        private void MoveToPlayer(GameObject player)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            m_rigidbody.AddForce(new Vector3(dir.x, 0f, 0f) * m_moveSpeed);
        }

        private void Attack()
        {
            if (m_playerController != null)
            {
                if (CanAttack())
                {
                    m_lastAttackTime = Time.time;

                    Shoot(m_playerController.gameObject);
                }
            }
        }

        private bool CanAttack()
        {
            return Time.time >= m_lastAttackTime + m_attackDelay;
        }

        public override void DealDamage(float damage)
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

        private void Shoot(GameObject target)
        {
            GameObject bulletObject = Instantiate(m_bulletPrefab.gameObject, transform.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            Vector3 direction = target.transform.position - transform.position;

            bullet.SetSpeed(m_speed);
            bullet.ApplyMovement(direction.normalized);
            bullet.SetDamage(m_damage);
        }
    }
}