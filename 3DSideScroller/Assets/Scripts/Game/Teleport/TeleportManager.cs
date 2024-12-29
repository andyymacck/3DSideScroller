using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


namespace SideScroller
{
    public class TeleportManager : MonoBehaviour
    {
        [SerializeField] private List<TeleportTrigger> m_teleportTriggers = new List<TeleportTrigger>();

        private bool m_isTeleporting = false;

        private void Start ()
        {
            m_teleportTriggers = FindObjectsOfType<TeleportTrigger>().ToList();

            EnableAllTeleports();
            InitTeleports();
        }

        private void InitTeleports()
        {
            foreach (TeleportTrigger teleportTrigger in m_teleportTriggers)
            {
                teleportTrigger.SetTeleportManager(this);
            }
        }

        private void EnableAllTeleports()
        {
            foreach (TeleportTrigger teleportTrigger in m_teleportTriggers)
            {
                teleportTrigger.EnableTeleportTrigger();
            }
        }

        private void DisableAllTeleports()
        {
            foreach (TeleportTrigger teleportTrigger in m_teleportTriggers)
            {
                teleportTrigger.DisableTeleportTrigger();
            }
        }

        public void TeleportPlayerObject(PlayerController player, Transform destination)
        {
            if (m_isTeleporting)
            {
                return;
            }

           StartCoroutine(TeleportCoroutine(player, destination));
        }

        private IEnumerator TeleportCoroutine(PlayerController player, Transform destination)
        {
            m_isTeleporting = true;

            EventHub.Instance.Publish(new ScreenFadeEvent(true));

            yield return new WaitForSeconds(3f);

            player.SetPosition(destination.position);
            CameraController.Instance.SetPosition(destination.position);
            CameraController.Instance.ZoomIn();
            EventHub.Instance.Publish(new ScreenFadeEvent(false));



            m_isTeleporting = false;

        }
    }
}