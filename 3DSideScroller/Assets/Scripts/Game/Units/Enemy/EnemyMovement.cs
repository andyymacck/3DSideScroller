using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

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
        [SerializeField] private Transform m_rootTransform;
        [SerializeField] private float m_moveSpeed = 100f;
        [SerializeField] private Waypoint[] m_wayPoints = new Waypoint[] { };

        [SerializeField] private PlatformMove m_loopMode;
        [SerializeField] private bool m_isMovingForward = true;

        private int m_platformIndex = 0;
        private bool m_isWaiting = false;
        private float m_waitTimer = 0f;
        private RotationStates m_rotationState;


        public void MoveToObject(GameObject gameObject)
        {
            Vector3 dir = (gameObject.transform.position - transform.position).normalized;
            m_rigidbody.AddForce(new Vector3(dir.x, 0f, 0f) * m_moveSpeed);

            SetRotation(gameObject.transform);
        }

        private void MoveToPosition(Vector3 position)
        {
            Vector3 dir = (position - transform.position).normalized;
            m_rigidbody.AddForce(new Vector3(dir.x, 0f, 0f) * m_moveSpeed);

            SetRotation(position);
        }

        public void SetRotation(RotationStates rotationStates)
        {
            if (m_rotationState == rotationStates)
            {
                return;
            }

            m_rotationState = rotationStates;
            Vector3 currentRotation = m_rootTransform.localRotation.eulerAngles;
            currentRotation.y = rotationStates == RotationStates.Right ? 0 : 180;
            m_rootTransform.localRotation = Quaternion.Euler(currentRotation);
        }

        public void SetRotation(Transform targetTransform)
        {
            if (targetTransform == null)
            {
                return;
            }

            SetRotation(targetTransform.position);
        }

        public void SetRotation(Vector3 position)
        {
            Vector3 dir = position - transform.position;

            RotationStates rotationState = dir.x > 0f ? RotationStates.Right : RotationStates.Left;
            SetRotation(rotationState);
        }

        public void Patrol()
        {
            if (m_isWaiting)
            {
                m_waitTimer += Time.deltaTime;

                if (m_waitTimer >= m_wayPoints[m_platformIndex].WaitTime)
                {
                    m_isWaiting = false;
                    m_waitTimer = 0f;
                }

                return; // Stop movement while waiting
            }

            Vector3 targetPosition = m_wayPoints[m_platformIndex].WaypointTranform.position;
            MoveToPosition(targetPosition);

            float distance = CalculateDistanceX(transform.position, targetPosition);
            if (distance < 0.1f)
            {
                m_isWaiting = true; // Start waiting
                DetermineNextWaypoint();
                m_rigidbody.velocity = Vector3.zero;
            }
        }

        private void DetermineNextWaypoint()
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

        private int GetRandomIndex()
        {
            return UnityEngine.Random.Range(0, m_wayPoints.Length);
        }

        private float CalculateDistanceX(Vector3 posA, Vector3 posB)
        {
            float distance = Math.Abs(posA.x - posB.x);
            return distance;
        }
    }

    [Serializable]
    public class Waypoint
    {
        public Transform WaypointTranform;
        public float WaitTime; // Time in seconds to wait at this waypoint
    }
}