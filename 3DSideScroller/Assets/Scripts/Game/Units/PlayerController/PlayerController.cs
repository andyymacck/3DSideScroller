using System;
using System.Collections;
using UnityEngine;

namespace SideScroller
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Unit
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private Transform m_transform;
        [SerializeField] private PlayerAnimationController m_animationController;
        [SerializeField] private GameObject m_weapon;
        [SerializeField] private float m_forceMovement = 2f;
        [SerializeField] private float m_forceJump = 2f;
        [SerializeField] private int m_attackDamage = 1;   // Define player's attack damage
        [SerializeField] private float m_distToFight = 2f; // cosnt
        [Space]
        [SerializeField] private PathAgent m_pathAgent;
        protected EnemyManager m_enemyManager;


        private int m_jumpLimit = 2; // max jumps before ground the player
        private int m_jumpCount = 0;
        private int m_jumpCountTotal = 0;
        private bool m_isGrounded = true;
        private bool m_isPunching = false;
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
            
            if (m_pathAgent == null)
            {
                m_pathAgent = GetComponent<PathAgent>();
                if (m_pathAgent == null)
                {
                    m_pathAgent = gameObject.AddComponent<PathAgent>();
                }
            }
        }

        private void Start()
        {
            EventHub.Instance.Subscribe<TeleportEvent>(OnTeleport);
            EventHub.Instance.Subscribe<LevelFinishedEvent>(OnLevelFinish);
            EventHub.Instance.Subscribe<CollectItemApprovalEvent>(CollectItemApprovalEvent);

            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthOnStart));

            if (m_pathAgent != null)
            {
                m_pathAgent.FindPositionOnPath(transform.position);
            }

            m_weapon.SetActive(false);
        }

        private void CollectItemApprovalEvent(CollectItemApprovalEvent eventData)
        {
            if (m_healthCurrent < m_healthMax && eventData.CollectableItem.Collectable.CollectabeType == CollectabeType.Health)
            {
                CollectableItem collectableItem = eventData.CollectableItem;
                Debug.Log($"CollectItemApprovalEvent {collectableItem.Collectable.CollectabeType}");
                eventData.Approve();
                Heal(collectableItem.Collectable.Count);
            }
        }

        void FixedUpdate()
        {
            if (m_state == PlayerStates.Win || m_pathAgent == null)
            {
                return;
            }

            float lookupRange = 2f;
            m_pathAgent.FindClosestPositionInRange(transform.position, lookupRange);

            Vector3 targetPosition = m_pathAgent.GetCurrentPosition();
            Vector3 targetDirection = m_pathAgent.GetCurrentDirection();

            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

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


            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) // Change key as needed
            {
                Punch();
            }
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
            
            if (m_pathAgent != null)
            {
                m_pathAgent.FindPositionOnPath(position);
            }
        }

        private void Punch()
        {
            if (!m_isPunching)
            {
                StartCoroutine(PunchCoroutine());
            }
        }

        private IEnumerator PunchCoroutine()
        {
            //Start of punch
            m_animationController.TrigerPunch();
            m_isPunching = true;

            yield return new WaitForSeconds(0.6f);

            //middle on punch

            m_weapon.SetActive(true);
            
            yield return new WaitForSeconds(0.3f);

            //end of punch
            m_weapon.SetActive(false);
            m_isPunching = false;
        }

        private void Jump()
        {
            if (m_jumpCount < m_jumpLimit || m_isGrounded)
            {
                Vector3 moveDir = Vector3.up;
                m_rigidbody.AddForce(moveDir * m_forceJump, ForceMode.Impulse);

                m_jumpCount++;
                m_jumpCountTotal++;
                SetState(PlayerStates.Jump);
                
                m_isGrounded = false;
                m_animationController.SetIsGrounded(false);
            }
        }

        private void Move(Vector3 direction)
        {
            m_rigidbody.AddForce(direction * m_forceMovement);

            if (m_pathAgent != null)
            {
                if (Vector3.Dot(direction, m_pathAgent.GetCurrentDirection()) > 0)
                    m_pathAgent.MoveForward(m_forceMovement * Time.deltaTime);
                else
                    m_pathAgent.MoveBackward(m_forceMovement * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsCollided(collision, Constants.GROUNG_TAG_ID) || IsCollided(collision, Constants.PLATFORM_TAG_ID))
            {
                m_jumpCount = 0;
                m_isGrounded = true;
                m_animationController.SetIsGrounded(true);
                
                if (m_state == PlayerStates.Jump)
                {
                    SetState(PlayerStates.Idle);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsCollided(collision, Constants.GROUNG_TAG_ID) || IsCollided(collision, Constants.PLATFORM_TAG_ID))
            {
                m_isGrounded = false;
                m_animationController.SetIsGrounded(false);
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

        public override void Heal(int amout)
        {
            m_healthCurrent += amout;

            if (m_healthCurrent > m_healthMax)
            {
                m_healthCurrent = m_healthMax;
            }

            EventHub.Instance.Publish(new HealthChangeEvent(m_healthCurrent, m_healthMax));
        }

        public override void DealDamage(int damage)
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
            EventHub.Instance.UnSubscribe<CollectItemApprovalEvent>(CollectItemApprovalEvent);
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