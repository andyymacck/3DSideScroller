using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SideScroller
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Unit
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private Transform m_transform;
        [SerializeField] private PlayerAnimationController m_animationController;
        [SerializeField] private float m_forceMovement = 2f;
        [SerializeField] private float m_forceJump = 2f;
        [SerializeField] private float m_attackDamage = 1f; // Define player's attack damage
        [SerializeField] private float m_distToFight = 2f; // cosnt
        [Space]
        [SerializeField] private CatmullRomGenPoints m_path;
        [SerializeField] private float m_pathPosition;
        [SerializeField] private float m_foundDist;
        [SerializeField] private float m_pathLength;
        protected EnemyManager m_enemyManager;


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

        private Unit m_currentEnemy;


        public void Initialize(EnemyManager manager)
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_healthCurrent = m_healthOnStart;

            SetEnemyManager(manager);
            SetState(PlayerStates.Idle);
        }

        private void Start()
        {
            EventHub.Instance.Subscribe<TeleportEvent>(OnTeleport);
            EventHub.Instance.Subscribe<LevelFinishedEvent>(OnLevelFinish);

            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthOnStart));

            m_path.GenWay();
            m_pathLength = m_path.GetPathLength();

            m_path.GetNearestPosOfDist(transform.position, out m_foundDist, out bool isFound, 1f);
            m_pathPosition = Math.Clamp(m_foundDist, 0f, m_pathLength);
        }

        void FixedUpdate()
        {
            if (m_state == PlayerStates.Win)
            {
                return;
            }

            float lookupRange = 2f;


            m_path.GetClosestPosOfDist(transform.position, m_pathPosition, lookupRange, out m_foundDist, out bool isFound, 0.1f);
            m_pathPosition = Math.Clamp(m_foundDist, 0f, m_pathLength);

            Vector3 targetPosition = m_path.GetPosByDist(m_pathPosition);
            Vector3 targetDirection = m_path.GetDirByDist(m_pathPosition);

            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

            //EnemyLookup();

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Move(targetDirection);
                SetState(PlayerStates.RunForward);
                m_animationController.SetRotation(targetDirection);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Move(-targetDirection);
                SetState(PlayerStates.RunForward);
                m_animationController.SetRotation(-targetDirection);
            }
            else if (m_isGrounded)
            {
                SetState(PlayerStates.Idle);
            }
        }

        private void Update()
        {
           if (m_state == PlayerStates.Win)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Space)) // Change key as needed
            {
                OnClickAttack();
            }

           //CalculateRotation();
        }

        public void SetEnemyManager(EnemyManager manager)
        {
            m_enemyManager = manager;
        }

        private void OnTeleport(TeleportEvent eventData)
        {
            SetPosition(eventData.Destination);
        }

        private void OnLevelFinish(LevelFinishedEvent eventData)
        {
            SetState(PlayerStates.Win);
            //start win animation
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void CalculateRotation()
        {
            if (m_currentEnemy != null)
            {
                Vector3 positionDiff = m_currentEnemy.transform.position - m_transform.position;
                RotationStates targetRotation = positionDiff.x > 0f ? RotationStates.Right : RotationStates.Left;
                m_animationController.SetRotation(targetRotation);
            }
            else
            {
                //m_animationController.SetRotation(RotationStates.Right);
            }
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

        private void Move(Vector3 direction)
        {
            m_rigidbody.AddForce(direction * m_forceMovement);
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


        private void EnemyLookup()
        {
            int enemyCount = m_enemyManager.EnemyList.Count;
            float minDist = m_distToFight;
            
            m_currentEnemy = null;

            for (int i = 0; i < enemyCount; i++)
            {
                Unit enemy = m_enemyManager.EnemyList[i];

                if (enemy == null) continue;

                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dist <= minDist)
                {
                    m_currentEnemy = enemy;
                    minDist = dist;
                }
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

            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthOnStart));

            if (m_isAlive == false)
            {
                Die();
            }
        }

        private void OnClickAttack()
        {
            if (CanAttack())
            {
                Debug.Log("ATTACK");
                //start hit animation
            }
            else
            {
                return;
            }

            if (IsEnemyInRange(m_currentEnemy, m_attackRange))
            {
                m_lastAttackTime = Time.time;
                m_currentEnemy.DealDamage(m_attackDamage);
                Debug.Log($"Deal damage: {m_currentEnemy}");
            }
        }

        private bool IsEnemyInRange(Unit enemy, float distanceToCheck)
        {
            if (enemy == null) return false;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
           return distance <= distanceToCheck;
        }

        private bool CanAttack()
        {
            return Time.time >= m_lastAttackTime + m_attackDelay;
        }

        public override void Die()
        {
            if (m_state == PlayerStates.Win)
            {
                return;
            }

            int score = 0;
            EventHub.Instance.Publish<GameOverEvent>(new GameOverEvent(score));
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<TeleportEvent>(OnTeleport);
            EventHub.Instance.UnSubscribe<LevelFinishedEvent>(OnLevelFinish);
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