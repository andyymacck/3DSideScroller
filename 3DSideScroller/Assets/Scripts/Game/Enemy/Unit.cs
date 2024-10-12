using UnityEngine;

namespace SideScroller
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected float m_health = 100f;

        protected bool m_isAlive = true;

        public bool IsAlive => m_isAlive;

        public abstract void DealDamage(float damage);

        public abstract void Die();
    }
}