using SideScroller;
using UnityEngine;

public class EnemyController : Unit
{
    [SerializeField] private float m_moveSpeed = 2f;
    [Header("Attack param")]
    [SerializeField] private float m_attackRange = 1f;
    [SerializeField] private float m_idleRange = 5f;
    [SerializeField] private float m_attackDelay = 1f;
    [SerializeField] private float m_damage = 1f;
    //[SerializeField] private float m_health = 100f;
    [SerializeField] private Rigidbody m_rigidbody;

    private GameObject m_playerObject;
    private PlayerController m_playerController;
    private float m_lastAttackTime = 0f;
    

    private void Start()
    {
        SetPlayer(GameObject.FindGameObjectWithTag(Constants.PlayerTagId));
    }


    private void Update()
    {
        if(m_playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_playerObject.transform.position);
            
            if(distanceToPlayer > m_idleRange)
            {
                return;
            }

            if(distanceToPlayer >= m_attackRange)
            {
                MoveToPlayer(m_playerObject);
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
            m_playerController.DealDamage(m_damage);
        }
    }
}