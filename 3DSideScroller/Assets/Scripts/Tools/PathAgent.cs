using UnityEngine;
using System.Collections.Generic;

namespace SideScroller
{
    /// <summary>
    /// Agent component for working with CatmullRomGenPoints path
    /// </summary>
    public class PathAgent : MonoBehaviour
    {
        [SerializeField] private CatmullRomGenPoints m_path;
        [SerializeField] private float m_pathPosition;
        [SerializeField] private float m_searchStep = 0.1f;
        [SerializeField] private float m_searchRange = 2f;

        [Tooltip("Select which axes to use for distance calculations")]
        [SerializeField] private DistanceCalculationMode m_distanceMode = DistanceCalculationMode.IgnoreY;

        // Path positions cache for faster lookup
        private List<Vector3> m_cachedPositions = new List<Vector3>();
        private float[] m_cachedDistances;
        private float m_pathLength;
        private int m_cacheSegmentCount = 100; // Number of segments for caching
        private bool m_isCacheReady = false;

        public float PathPosition => m_pathPosition;
        public float PathLength => m_pathLength;

        private void Awake()
        {
            if (m_path != null)
            {
                // Sync distance mode with path
                m_path.SetDistanceMode(m_distanceMode);
                BuildCache();
            }
        }

        /// <summary>
        /// Initialize agent with the specified path
        /// </summary>
        public void Initialize(CatmullRomGenPoints path)
        {
            m_path = path;
            // Sync distance mode with path
            if (m_path != null)
            {
                m_path.SetDistanceMode(m_distanceMode);
            }
            BuildCache();
        }

        /// <summary>
        /// Set distance calculation mode
        /// </summary>
        public void SetDistanceMode(DistanceCalculationMode mode)
        {
            if (m_distanceMode != mode)
            {
                m_distanceMode = mode;

                // Sync with path
                if (m_path != null)
                {
                    m_path.SetDistanceMode(mode);
                }

                // Rebuild cache with new distance mode
                BuildCache();
            }
        }

        /// <summary>
        /// Get current distance calculation mode
        /// </summary>
        public DistanceCalculationMode GetDistanceMode()
        {
            return m_distanceMode;
        }

        /// <summary>
        /// Calculate distance between points based on selected mode
        /// </summary>
        public float CalculateDistance(Vector3 a, Vector3 b)
        {
            if (m_path != null)
            {
                // Use path's distance calculation if available
                return m_path.CalculateDistance(a, b);
            }

            // Fallback to own calculation if path not available
            switch (m_distanceMode)
            {
                case DistanceCalculationMode.ThreeDimensional:
                    return Vector3.Distance(a, b);

                case DistanceCalculationMode.IgnoreX:
                    float dy_yz = a.y - b.y;
                    float dz_yz = a.z - b.z;
                    return Mathf.Sqrt(dy_yz * dy_yz + dz_yz * dz_yz);

                case DistanceCalculationMode.IgnoreY:
                    float dx_xz = a.x - b.x;
                    float dz_xz = a.z - b.z;
                    return Mathf.Sqrt(dx_xz * dx_xz + dz_xz * dz_xz);

                case DistanceCalculationMode.IgnoreZ:
                    float dx_xy = a.x - b.x;
                    float dy_xy = a.y - b.y;
                    return Mathf.Sqrt(dx_xy * dx_xy + dy_xy * dy_xy);

                default:
                    return Vector3.Distance(a, b);
            }
        }

        /// <summary>
        /// Build position cache for fast lookups
        /// </summary>
        public void BuildCache()
        {
            if (m_path == null || !m_path.IsWayReady())
            {
                Debug.LogWarning("Path is not ready for caching!");
                return;
            }

            m_pathLength = m_path.GetPathLength();
            m_cachedPositions.Clear();

            // Create cache with uniform step
            m_cachedDistances = new float[m_cacheSegmentCount + 1];
            float distStep = m_pathLength / m_cacheSegmentCount;

            for (int i = 0; i <= m_cacheSegmentCount; i++)
            {
                float dist = i * distStep;
                m_cachedDistances[i] = dist;
                m_cachedPositions.Add(m_path.GetPosByDist(dist));
            }

            m_isCacheReady = true;

            // Determine initial agent position on the path
            FindPositionOnPath(transform.position);
        }

        /// <summary>
        /// Get the nearest position on the path
        /// </summary>
        public void FindPositionOnPath(Vector3 position)
        {
            if (!m_isCacheReady)
            {
                BuildCache();
                if (!m_isCacheReady) return;
            }

            // First use cache for approximate search
            int closestIndex = 0;
            float closestDist = float.MaxValue;

            for (int i = 0; i < m_cachedPositions.Count; i++)
            {
                float dist = CalculateDistance(position, m_cachedPositions[i]);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }

            // Get approximate distance
            float approximateDist = m_cachedDistances[closestIndex];

            // Now do a more precise search in the local area
            float searchStart = Mathf.Max(0, approximateDist - m_searchRange);
            float searchEnd = Mathf.Min(m_pathLength, approximateDist + m_searchRange);

            float bestDist = closestDist;
            float bestPathDist = approximateDist;

            for (float dist = searchStart; dist <= searchEnd; dist += m_searchStep)
            {
                Vector3 posOnPath = m_path.GetPosByDist(dist);
                float distToObj = CalculateDistance(position, posOnPath);

                if (distToObj < bestDist)
                {
                    bestDist = distToObj;
                    bestPathDist = dist;
                }
            }

            m_pathPosition = Mathf.Clamp(bestPathDist, 0, m_pathLength);
        }

        /// <summary>
        /// Find the closest position within a given range from the current position
        /// </summary>
        public void FindClosestPositionInRange(Vector3 position, float range)
        {
            if (!m_isCacheReady)
            {
                BuildCache();
                if (!m_isCacheReady) return;
            }

            float searchStart = Mathf.Max(0, m_pathPosition - range);
            float searchEnd = Mathf.Min(m_pathLength, m_pathPosition + range);

            float bestDist = float.MaxValue;
            float bestPathDist = m_pathPosition;

            for (float dist = searchStart; dist <= searchEnd; dist += m_searchStep)
            {
                Vector3 posOnPath = m_path.GetPosByDist(dist);
                float distToObj = CalculateDistance(position, posOnPath);

                if (distToObj < bestDist)
                {
                    bestDist = distToObj;
                    bestPathDist = dist;
                }
            }

            m_pathPosition = Mathf.Clamp(bestPathDist, 0, m_pathLength);
        }

        /// <summary>
        /// Move forward along the path by the specified distance
        /// </summary>
        public void MoveForward(float distance)
        {
            m_pathPosition += distance;
            m_pathPosition = Mathf.Clamp(m_pathPosition, 0, m_pathLength);
        }

        /// <summary>
        /// Move backward along the path by the specified distance
        /// </summary>
        public void MoveBackward(float distance)
        {
            m_pathPosition -= distance;
            m_pathPosition = Mathf.Clamp(m_pathPosition, 0, m_pathLength);
        }

        /// <summary>
        /// Get current position on the path
        /// </summary>
        public Vector3 GetCurrentPosition()
        {
            if (m_path == null) return transform.position;
            return m_path.GetPosByDist(m_pathPosition);
        }

        /// <summary>
        /// Get current direction on the path
        /// </summary>
        public Vector3 GetCurrentDirection()
        {
            if (m_path == null) return transform.forward;
            return m_path.GetDirByDist(m_pathPosition);
        }

        /// <summary>
        /// Get current position on the path with X offset
        /// </summary>
        public Vector3 GetCurrentPositionWithXOffset(float offsetX)
        {
            if (m_path == null) return transform.position;
            return m_path.GetPosByDistWithXOffset(m_pathPosition, offsetX);
        }

        /// <summary>
        /// Get path progress (0-1)
        /// </summary>
        public float GetPathProgress()
        {
            return m_pathPosition / m_pathLength;
        }

        /// <summary>
        /// Set position on the path by relative value (0-1)
        /// </summary>
        public void SetPositionByFactor(float factor)
        {
            m_pathPosition = Mathf.Clamp01(factor) * m_pathLength;
        }

        public void OnDrawGizmosSelected()
        {
            if (m_path != null && m_isCacheReady)
            {
                // Draw current position on the path
                Gizmos.color = Color.green;
                Vector3 currentPos = GetCurrentPosition();
                Gizmos.DrawSphere(currentPos, 0.3f);

                // Draw cached points
                Gizmos.color = Color.cyan;
                for (int i = 0; i < m_cachedPositions.Count; i++)
                {
                    Gizmos.DrawSphere(m_cachedPositions[i], 0.1f);
                }

                // Visualize which dimensions are being used
                Gizmos.color = Color.yellow;
                Vector3 center = currentPos;
                float size = 1f;

                switch (m_distanceMode)
                {
                    case DistanceCalculationMode.IgnoreX:
                        // Draw YZ plane
                        Gizmos.DrawLine(
                            new Vector3(center.x, center.y - size / 2, center.z - size / 2),
                            new Vector3(center.x, center.y + size / 2, center.z - size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x, center.y + size / 2, center.z - size / 2),
                            new Vector3(center.x, center.y + size / 2, center.z + size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x, center.y + size / 2, center.z + size / 2),
                            new Vector3(center.x, center.y - size / 2, center.z + size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x, center.y - size / 2, center.z + size / 2),
                            new Vector3(center.x, center.y - size / 2, center.z - size / 2)
                            );
                        break;

                    case DistanceCalculationMode.IgnoreY:
                        // Draw XZ plane
                        Gizmos.DrawLine(
                            new Vector3(center.x - size / 2, center.y, center.z - size / 2),
                            new Vector3(center.x + size / 2, center.y, center.z - size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x + size / 2, center.y, center.z - size / 2),
                            new Vector3(center.x + size / 2, center.y, center.z + size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x + size / 2, center.y, center.z + size / 2),
                            new Vector3(center.x - size / 2, center.y, center.z + size / 2)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x - size / 2, center.y, center.z + size / 2),
                            new Vector3(center.x - size / 2, center.y, center.z - size / 2)
                            );
                        break;

                    case DistanceCalculationMode.IgnoreZ:
                        // Draw XY plane
                        Gizmos.DrawLine(
                            new Vector3(center.x - size / 2, center.y - size / 2, center.z),
                            new Vector3(center.x + size / 2, center.y - size / 2, center.z)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x + size / 2, center.y - size / 2, center.z),
                            new Vector3(center.x + size / 2, center.y + size / 2, center.z)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x + size / 2, center.y + size / 2, center.z),
                            new Vector3(center.x - size / 2, center.y + size / 2, center.z)
                            );
                        Gizmos.DrawLine(
                            new Vector3(center.x - size / 2, center.y + size / 2, center.z),
                            new Vector3(center.x - size / 2, center.y - size / 2, center.z)
                            );
                        break;

                    case DistanceCalculationMode.ThreeDimensional:
                        // Draw a wire cube to indicate 3D calculation
                        Gizmos.DrawWireCube(center, new Vector3(size, size, size));
                        break;
                }
            }
        }
    }
}