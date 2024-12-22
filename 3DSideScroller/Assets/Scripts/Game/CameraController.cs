using UnityEngine;

namespace SideScroller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerController m_player;
        [SerializeField] private Transform m_transform;
        [SerializeField] private Vector3 m_cameraOffset;
        [SerializeField] private Vector3 m_cameraOffsetDeath;
        [SerializeField] private Vector3 m_cameraOffsetStart;
        [SerializeField] private float m_speedVertical = 1f;
        [SerializeField] private float m_speedHorizontal = 1f;
        [SerializeField] private float m_playerSpeedOffsetFactor = 1f;
        [SerializeField] private float m_cameraLimitZOnDeath = 5f;

        private Vector3 m_lastPlayerPosition;

        public static CameraController Instance;

        private void Awake()
        {
            m_transform.position = m_player.transform.position + m_cameraOffsetStart;
            Instance = this;
        }

        void Start()
        {

        }

        void Update()
        {
            if (m_player == null)
            {
                Vector3 tartgetPostDeath = m_lastPlayerPosition + m_cameraOffsetDeath;
                MoveCamera(tartgetPostDeath); 

                return;
            }

            Vector3 distance = m_player.transform.position - m_transform.position;
            Vector3 targetPos = m_player.transform.position + m_cameraOffset;
            float targetX = targetPos.x + (m_player.PlayerVelocity.x * m_playerSpeedOffsetFactor);
            m_lastPlayerPosition = m_player.transform.position;

            MoveCamera(new Vector3(targetX, targetPos.y, targetPos.z)); 
        }

        private void MoveCamera(Vector3 targetPos)
        {
            float x = Mathf.Lerp(m_transform.transform.position.x, targetPos.x, Time.deltaTime * m_speedHorizontal);
            float y = Mathf.Lerp(m_transform.transform.position.y, targetPos.y, Time.deltaTime * m_speedVertical);
            float z = Mathf.Lerp(m_transform.transform.position.z, targetPos.z, Time.deltaTime);
            m_transform.position = new Vector3(x, y, z);
        }

        public void SetPosition(Vector3 targetPosition)
        {
            Vector3 position = targetPosition + m_cameraOffset;
            m_transform.position = position;
        }

        public void ZoomIn()
        {
            Vector3 currentPos = m_transform.position;
            currentPos.z = m_cameraOffsetDeath.z;
            m_transform.position = currentPos;
        }
    }
}