using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected float m_healthCurrent = 100f;
        [SerializeField] protected float m_healthOnStart = 100f;
        
        [SerializeField] protected float m_attackDelay = 1f;
        [SerializeField] protected float m_attackRange = 1f;

        protected float m_lastAttackTime = 0f;
        protected bool m_isAlive = true;
        
        public bool IsAlive => m_isAlive;

        public abstract void DealDamage(float damage);

        public abstract void Die();

    }
}