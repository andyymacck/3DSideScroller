using UnityEngine;


namespace SideScroller
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private Transform m_targetDestination;
        private TeleportManager m_teleportManager;
        private bool m_isEnabled = false;

        private void OnTriggerEnter(Collider other)
        {
            if (m_isEnabled == false)
            {
                return;
            }

            if (other.CompareTag(Constants.PLAYER_TAG_ID))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

                if (playerController != null)
                {
                    m_teleportManager.TeleportPlayerObject(playerController, m_targetDestination);
                }
            }
        }

        public void SetTeleportManager(TeleportManager teleportManager)
        {
            m_teleportManager = teleportManager;
        }

        public void EnableTeleportTrigger()
        {
            m_isEnabled = true;
        }

        public void DisableTeleportTrigger()
        {
            m_isEnabled = false;
        }
    }
}