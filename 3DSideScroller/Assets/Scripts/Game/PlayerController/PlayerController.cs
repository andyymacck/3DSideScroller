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

        public Vector3 PlayerVelocity => m_rigidbody.velocity;


        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {

        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Move(false);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Move(true);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }

        private void Jump()
        {
            if ((m_jumpCount < m_jumpLimit) || (m_isGrounded))
            {
                Vector3 moveDir = Vector3.up;
                m_rigidbody.AddForce(moveDir * m_forceJump, ForceMode.Impulse);

                m_jumpCount++; // m_jumpCount = m_jumpCount + 1
                m_jumpCountTotal++;

                EventHub.Instance.Publish(new JumpEvent(m_jumpCountTotal));
            }
        }

        private void Move(bool isLeft)
        {
            Vector3 moveDir = isLeft ? Vector3.left : Vector3.right;
            m_rigidbody.AddForce(moveDir * m_forceMovement);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsCollided(collision, Constants.GroundTagId) || IsCollided(collision, Constants.PlatformTagId))
            {
                m_jumpCount = 0;
                m_isGrounded = true;
                EventHub.Instance.Publish(new IsGroundedEvent(true));
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsCollided(collision, Constants.GroundTagId) || IsCollided(collision, Constants.PlatformTagId))
            {
                m_isGrounded = false;
                EventHub.Instance.Publish(new IsGroundedEvent(false));
            }
        }

        private bool IsCollided(Collision collision, string name)
        {
            return collision.gameObject.CompareTag(name);
        }
    }
}