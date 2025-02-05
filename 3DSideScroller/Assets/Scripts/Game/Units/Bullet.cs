using UnityEngine;

namespace SideScroller
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidBody;
        [SerializeField] private float m_lifeTime = 4f;
        [SerializeField] private float m_damage = 5f;
        [SerializeField] private float m_speed = 5f;

        void Start()
        {
            Destroy(gameObject, 3f);
        }

        public void ApplyMovement(Vector3 direction)
        {
            m_rigidBody.velocity = direction * m_speed;
        }

        public void SetDamage(float damage)
        {
            m_damage = damage;
        }

        public void SetSpeed(float speed)
        {
            m_speed = speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG_ID))
            {
                Unit unit = other.GetComponent<Unit>();

                if (unit != null)
                {
                    unit.DealDamage(m_damage);
                }

                Destroy(gameObject);
            }
        }
    }
}