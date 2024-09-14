using UnityEngine;

namespace SideScroller
{

    public class MovedPlatform : MonoBehaviour
    {
        private enum PlatformMove
        {
            Loop,
            PingPong,
            Random
        }

        [SerializeField] private Transform m_platform;
        [SerializeField] private Transform[] m_wayPoints = new Transform[] { };
        [SerializeField] private float m_speed = 2f;
        [SerializeField] private PlatformMove m_loopMode;
        [SerializeField] private bool m_isMovingForward = true;


        private int m_platformIndex = 0;


        private void Awake()
        {
            if (m_wayPoints == null && m_wayPoints.Length == 0)
            {
                return;
            }

            m_platform.position = m_wayPoints[0].position;
        }

        void FixedUpdate()
        {
            Vector3 targetPosition = m_wayPoints[m_platformIndex].position;

            m_platform.position = Vector3.MoveTowards(m_platform.position, targetPosition, m_speed * Time.fixedDeltaTime);
            float distance = Vector3.Distance(m_platform.position, targetPosition);

            if (distance < 0.1f)
            {
                if (m_loopMode == PlatformMove.Random)
                {
                    m_platformIndex = GetRandomIndex();
                    return;
                }

                if (m_isMovingForward)
                {
                    m_platformIndex++;

                    if (m_platformIndex >= m_wayPoints.Length)
                    {
                        if (m_loopMode == PlatformMove.Loop)
                        {
                            m_platformIndex = 0;
                        }
                        else if (m_loopMode == PlatformMove.PingPong)
                        {
                            m_platformIndex = m_wayPoints.Length - 1;
                            m_isMovingForward = false;
                        }
                    }
                }
                else
                {
                    m_platformIndex--;

                    if (m_platformIndex < 0)
                    {
                        m_platformIndex = 1;
                        m_isMovingForward = true;
                    }
                }
            }
        }

        private int GetRandomIndex()
        {
            return Random.Range(0, m_wayPoints.Length);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision != null && collision.gameObject.CompareTag(Constants.PlayerTagId))
            {
                collision.gameObject.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision != null && collision.gameObject.CompareTag(Constants.PlayerTagId))
            {
                collision.gameObject.transform.SetParent(null);
            }
        }
    }
}