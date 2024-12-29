using System;
using UnityEngine;

namespace SideScroller
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Unit
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private float m_forceMovement = 2f;
        [SerializeField] private float m_forceJump = 2f;

        private int m_jumpLimit = 2; // max jumps before ground the player
        private int m_jumpCount = 0;
        private int m_jumpCountTotal = 0;
        private bool m_isGrounded = true;
        private PlayerStates m_state;

        public Vector3 PlayerVelocity => m_rigidbody.velocity;
        public PlayerStates State => m_state;

        public Action<Collider> OntriggerEnterEvent;
        public Action<Collider> OntriggerExitEvent;
        public Action<PlayerStates> OnplayerStateChangedEvent;
        

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_healthCurrent = m_healthOnStart;
        }

        private void Start()
        {
            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthOnStart));

            SetState(PlayerStates.Idle);
        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Move(false);
                SetState(PlayerStates.RunForward);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Move(true);
                SetState(PlayerStates.RunBackwards);
            }
            else if (m_isGrounded)
            {
                SetState(PlayerStates.Idle);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void Jump()
        {
            if ((m_jumpCount < m_jumpLimit) || (m_isGrounded))
            {
                Vector3 moveDir = Vector3.up;
                m_rigidbody.AddForce(moveDir * m_forceJump, ForceMode.Impulse);

                m_jumpCount++;
                m_jumpCountTotal++;
                SetState(PlayerStates.Jump);
            }
        }

        private void Move(bool isLeft)
        {
            Vector3 moveDir = isLeft ? Vector3.left : Vector3.right;
            m_rigidbody.AddForce(moveDir * m_forceMovement);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsCollided(collision, Constants.GROUNG_TAG_ID) || IsCollided(collision, Constants.PLATFORM_TAG_ID))
            {
                m_jumpCount = 0;
                m_isGrounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsCollided(collision, Constants.GROUNG_TAG_ID) || IsCollided(collision, Constants.PLATFORM_TAG_ID))
            {
                m_isGrounded = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OntriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OntriggerExitEvent?.Invoke(other);
        }

        private bool IsCollided(Collision collision, string name)
        {
            return collision.gameObject.CompareTag(name);
        }

        public override void DealDamage(float damage)
        {
            if (!m_isAlive)
            {
                return;
            }

            m_healthCurrent -= damage;
            m_isAlive = m_healthCurrent > 0f;

            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthOnStart));

            if (m_isAlive == false)
            {
                Die();
            }
        }

        public override void Die()
        {
            Destroy(gameObject);
        }

        private void StartDieFX()
        {

        }

        private void SetState(PlayerStates state)
        {
            if (m_state != state)
            {
                m_state = state;
                OnplayerStateChangedEvent?.Invoke(state);
            }
        }
    }
}