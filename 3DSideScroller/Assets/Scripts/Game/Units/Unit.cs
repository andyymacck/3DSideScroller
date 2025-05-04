using UnityEngine;

namespace SideScroller
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected int m_healthMax = 5;
        [SerializeField] protected int m_healthCurrent = 1;
        [SerializeField] protected int m_healthOnStart = 1;
        
        [SerializeField] protected float m_attackDelay = 1f;
        [SerializeField] protected float m_attackRange = 1f;

        protected float m_lastAttackTime = 0f;
        protected bool m_isAlive = true;
        
        public bool IsAlive => m_isAlive;

        public abstract void DealDamage(int damage);

        public abstract void Die();

    }
}