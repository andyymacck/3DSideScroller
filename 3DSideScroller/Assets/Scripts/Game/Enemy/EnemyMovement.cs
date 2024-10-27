using UnityEngine;

namespace SideScroller
{
    public class EnemyMovement : MonoBehaviour
    {
        private enum PlatformMove
        {
            Loop,
            PingPong,
            Random
        }

        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private float m_moveSpeed = 100f;
        [SerializeField] private Transform[] m_wayPoints = new Transform[] { };

        [SerializeField] private PlatformMove m_loopMode;
        [SerializeField] private bool m_isMovingForward = true;

        private int m_platformIndex = 0;


        void Start()
        {

        }

        void Update()
        {

        }

        public void MoveToObject(GameObject gameObject)
        {
            Vector3 dir = (gameObject.transform.position - transform.position).normalized;
            m_rigidbody.AddForce(new Vector3(dir.x, 0f, 0f) * m_moveSpeed);
        }

        private void MoveToPosition(Vector3 position)
        {
            Vector3 dir = (position - transform.position).normalized;
            m_rigidbody.AddForce(new Vector3(dir.x, 0f, 0f) * m_moveSpeed);
        }

        //Delay to stay on waypoit (waypoint wait time)
        //expant waypoint data thrue using custom waypoint class (create own waypoint class with parameter
        //and use intead of Transform[] m_wayPoints

        public void Patrol()
        {
            Vector3 targetPosition = m_wayPoints[m_platformIndex].position;

            MoveToPosition(targetPosition);
            //Extract and specify axis options
            float distance = Vector3.Distance(transform.position, targetPosition);

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
    }
}