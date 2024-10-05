using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float m_health = 100f;

    private bool m_isAlive = true;

    public bool IsAlive => m_isAlive;


    public void DealDamage(float damage)
    {
        if (!m_isAlive)
        {
            return;
        }

        m_health -= damage;
        m_isAlive = m_health > 0f;

        if(m_isAlive == false)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
