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
        [SerializeField] private float m_speedZ = 1f;
        [SerializeField] private float m_playerSpeedOffsetFactor = 1f;
        [SerializeField] private float m_playerSpeedOffsetFactorZ = 1f;

        private Vector3 m_lastPlayerPosition;
        private Vector3 m_cameraTargetPosition;

        public static CameraController Instance;

        private void Awake()
        {
            EventHub.Instance.Subscribe<GameOverEvent>(OnGameOver);
            EventHub.Instance.Subscribe<TeleportEvent>(OnTeleport);
            // Start with a fixed z position using m_cameraOffsetStart
            Vector3 startPos = m_player.transform.position + m_cameraOffsetStart;
            startPos.z = m_cameraOffsetStart.z;
            m_transform.position = startPos;
            Instance = this;
        }

        void Update()
        {
            if (m_player != null) // May be expanded later for different camera states.
            {
                HandlePlayerPosition();
            }

            MoveCamera(m_cameraTargetPosition);
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<TeleportEvent>(OnTeleport);
            EventHub.Instance.UnSubscribe<GameOverEvent>(OnGameOver);
        }

        private void HandlePlayerPosition()
        {
            Vector3 targetPos = m_player.transform.position + m_cameraOffset;
            float targetX = targetPos.x + (m_player.PlayerVelocity.x * m_playerSpeedOffsetFactor);
            float targetZ = targetPos.z + (m_player.PlayerVelocity.z * m_playerSpeedOffsetFactorZ);
            m_lastPlayerPosition = m_player.transform.position;
            m_cameraTargetPosition = new Vector3(targetX, targetPos.y, targetZ);
        }

        private void MoveCamera(Vector3 targetPos)
        {
            float x = Mathf.Lerp(m_transform.position.x, targetPos.x, Time.deltaTime * m_speedHorizontal);
            float y = Mathf.Lerp(m_transform.position.y, targetPos.y, Time.deltaTime * m_speedVertical);
            float z = Mathf.Lerp(m_transform.position.z, targetPos.z, Time.deltaTime * m_speedZ);
            m_transform.position = new Vector3(x, y, z);
        }

        private void OnTeleport(TeleportEvent eventData)
        {
            SetPosition(eventData.Destination);
            ZoomIn();
        }

        private void OnGameOver(GameOverEvent eventData)
        {
            // Set the camera target using the death offset, but force the z value
            m_cameraTargetPosition = new Vector3(
                m_lastPlayerPosition.x + m_cameraOffsetDeath.x,
                m_lastPlayerPosition.y + m_cameraOffsetDeath.y,
                m_cameraOffsetDeath.z);
        }

        public void SetPosition(Vector3 targetPosition)
        {
            Vector3 position = targetPosition + m_cameraOffset;
            // Force the z position to the one defined in m_cameraOffset
            position.z = m_cameraOffset.z;
            m_transform.position = position;
        }

        public void ZoomIn()
        {
            Vector3 currentPos = m_transform.position;
            // Adjust the z value to use the death offset z
            currentPos.z = m_cameraOffsetDeath.z;
            m_transform.position = currentPos;
        }
    }
}
