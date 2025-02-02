using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.Timeline.AnimationPlayableAsset;

namespace SideScroller
{
    public class EnemyLaser : Unit
    {
        [SerializeField] private float m_moveSpeed = 2f;
        [Header("Attack parameters")]
        [SerializeField] private float m_idleRange = 12f; // Distance to stop chasing
        [SerializeField] private float m_damage = 5f; // Damage per laser
        [Space]
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private LayerMask m_layerMask;

        private GameObject m_playerObject;
        private Unit m_playerController;
        private EnemyManager m_enemyManager;

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
                    return; // Outside of idle range, stop acting
                }

                if (distanceToPlayer < m_attackRange)
                {
                    Attack();
                }
                else
                {
                    MoveToPlayer(m_playerObject);
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
            transform.position += dir * m_moveSpeed * Time.deltaTime;
        }

        private void Attack()
        {
            if (m_playerController != null && Time.time >= m_lastAttackTime + m_attackDelay)
            {
                m_lastAttackTime = Time.time;

                Shoot(m_playerController.gameObject);
            }
        }

        public override void DealDamage(float damage)
        {
            if (!m_isAlive)
            {
                return;
            }

            m_healthCurrent -= damage;
            m_isAlive = m_healthCurrent > 0f;

            if (!m_isAlive)
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
            Vector3 direction = transform.right;

            bool isHit = Physics.Raycast(transform.position, direction, out RaycastHit hit, m_attackRange, m_layerMask);

            m_lineRenderer.enabled = true;
            m_lineRenderer.SetPosition(0, transform.position);

            if (isHit)
            {
                m_lineRenderer.SetPosition(1, hit.point);

                Unit unit = hit.collider.GetComponent<Unit>();

                if (unit != null)
                {
                    unit.DealDamage(m_damage);
                }
            }
            else
            {
                m_lineRenderer.SetPosition(1, transform.position + direction * m_attackRange);
            }

            StartCoroutine(DisableLaserVisual());
        }

        private IEnumerator DisableLaserVisual()
        {
            yield return new WaitForSeconds(0.3f);
            m_lineRenderer.enabled = false;
        }
    }
}