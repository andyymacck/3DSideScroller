using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    [ExecuteInEditMode]
    public class CatmullRomGenPoints : MonoBehaviour
    {
        [Header("Editor param")]
        [SerializeField] private bool m_rebuildWay = false;
        [SerializeField] private float m_step = 1f;
        [SerializeField] private int m_div;
        [SerializeField] private float m_length;
        [SerializeField] private bool m_useSpatialPartitioning = true; // Enable spatial partitioning for optimization

        [Header("Distance Calculation")]
        [Tooltip("Select which axes to use for distance calculations")]
        [SerializeField] private DistanceCalculationMode m_distanceMode = DistanceCalculationMode.IgnoreY;

        private Vector3[] m_points = new Vector3[0];

        // Data caching for faster lookups
        private Vector3[] m_cachedPointsForLookup;
        private float[] m_cachedDistancesForLookup;

        // Spatial partitioning for fast nearest point search
        private Dictionary<Vector3Int, List<int>> m_spatialGrid;
        private float m_cellSize = 5f; // Cell size for spatial partitioning

        private void Awake()
        {
            GenWay();
        }

        public void GenWay()
        {
            Transform root = transform;

            CheckPoints(root);

            float rawLength = 0f;

            for (int i = 1; i < root.childCount - 1; ++i)
            {
                rawLength += CalculateDistance(root.GetChild(i).position, root.GetChild(i + 1).position);
            }

            m_div = (int)(rawLength / 2f);

            List<Vector3> points = new List<Vector3>();

            for (int i = 1; i < root.childCount - 2; ++i)
            {
                Vector3 p0 = root.GetChild(i - 1).position;
                Vector3 p1 = root.GetChild(i).position;
                Vector3 p2 = root.GetChild(i + 1).position;
                Vector3 p3 = root.GetChild(i + 2).position;

                for (int j = i > 1 ? 1 : 0; j <= m_div; ++j)
                    points.Add(GetCatmullRomPosition(j / (float)m_div, p0, p1, p2, p3));
            }

            m_points = points.ToArray();
            m_length = CalcLength(points);

            // Create point cache for faster lookups
            BuildPointsCache();

            // Create spatial partitioning if enabled
            if (m_useSpatialPartitioning)
            {
                BuildSpatialGrid();
            }
        }

        /// <summary>
        /// Calculate distance between two points based on selected distance mode
        /// </summary>
        public float CalculateDistance(Vector3 a, Vector3 b)
        {
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
        /// Get current distance calculation mode
        /// </summary>
        public DistanceCalculationMode GetDistanceMode()
        {
            return m_distanceMode;
        }

        /// <summary>
        /// Set distance calculation mode and rebuild path
        /// </summary>
        public void SetDistanceMode(DistanceCalculationMode mode)
        {
            if (m_distanceMode != mode)
            {
                m_distanceMode = mode;
                GenWay();
            }
        }

        /// <summary>
        /// Build point cache for faster lookups
        /// </summary>
        private void BuildPointsCache()
        {
            int cacheSize = Mathf.Min(1000, m_points.Length); // Limit cache size

            m_cachedPointsForLookup = new Vector3[cacheSize];
            m_cachedDistancesForLookup = new float[cacheSize];

            float distStep = m_length / (cacheSize - 1);

            for (int i = 0; i < cacheSize; i++)
            {
                float dist = i * distStep;
                m_cachedDistancesForLookup[i] = dist;
                m_cachedPointsForLookup[i] = GetPosByDistUncached(dist);
            }
        }

        /// <summary>
        /// Build spatial grid for fast nearest point search
        /// </summary>
        private void BuildSpatialGrid()
        {
            m_spatialGrid = new Dictionary<Vector3Int, List<int>>();

            for (int i = 0; i < m_cachedPointsForLookup.Length; i++)
            {
                Vector3Int cell = GetCellForPosition(m_cachedPointsForLookup[i]);

                if (!m_spatialGrid.TryGetValue(cell, out List<int> indices))
                {
                    indices = new List<int>();
                    m_spatialGrid[cell] = indices;
                }

                indices.Add(i);
            }
        }

        /// <summary>
        /// Get cell for position in spatial partitioning based on distance calculation mode
        /// </summary>
        private Vector3Int GetCellForPosition(Vector3 position)
        {
            switch (m_distanceMode)
            {
                case DistanceCalculationMode.ThreeDimensional:
                    return new Vector3Int(
                        Mathf.FloorToInt(position.x / m_cellSize),
                        Mathf.FloorToInt(position.y / m_cellSize),
                        Mathf.FloorToInt(position.z / m_cellSize)
                        );

                case DistanceCalculationMode.IgnoreX:
                    return new Vector3Int(
                        0, // Ignore X
                        Mathf.FloorToInt(position.y / m_cellSize),
                        Mathf.FloorToInt(position.z / m_cellSize)
                        );

                case DistanceCalculationMode.IgnoreY:
                    return new Vector3Int(
                        Mathf.FloorToInt(position.x / m_cellSize),
                        0, // Ignore Y
                        Mathf.FloorToInt(position.z / m_cellSize)
                        );

                case DistanceCalculationMode.IgnoreZ:
                    return new Vector3Int(
                        Mathf.FloorToInt(position.x / m_cellSize),
                        Mathf.FloorToInt(position.y / m_cellSize),
                        0 // Ignore Z
                        );

                default:
                    return new Vector3Int(
                        Mathf.FloorToInt(position.x / m_cellSize),
                        Mathf.FloorToInt(position.y / m_cellSize),
                        Mathf.FloorToInt(position.z / m_cellSize)
                        );
            }
        }

        private void CheckPoints(Transform root)
        {
            if (root.childCount < 3)
            {
                Debug.LogError("NEED MORE POINTS");

                for (int i = 0; i < 3; i++)
                {
                    GameObject go = new GameObject("waypoint");
                    go.transform.parent = transform;
                }
            }

            // Add guide points
            if (root.childCount == 3)
            {
                GameObject go = new GameObject("Guide Point");
                go.transform.SetParent(root);
                go.transform.SetAsFirstSibling();
                go.transform.position = root.GetChild(1).position + (root.GetChild(1).position - root.GetChild(2).position) / 2f;

                GameObject go2 = new GameObject("Guide Point");
                go2.transform.SetParent(root);
                go2.transform.SetAsLastSibling();
                go2.transform.position = root.GetChild(3).position + (root.GetChild(3).position - root.GetChild(2).position) / 2f;
            }
        }

        private void CheckNames()
        {
            for (int i = 1; i < transform.childCount - 1; ++i)
            {
                transform.GetChild(i).name = i.ToString();
            }

            transform.GetChild(0).name = transform.GetChild(transform.childCount - 1).name = "Guide Point";
        }

        private Vector3[] GetPoints(float stepLength)
        {
            if (m_points.Length == 0)
            {
                Debug.LogError("Way not calculated");
                return new Vector3[0];
            }

            float length = 0f;
            float target = 0f;

            int stepsC = (int)(m_length / stepLength);
            float step = m_length / (float)(stepsC + 1);

            List<Vector3> result = new List<Vector3>();

            for (int i = 1; i < m_points.Length; ++i)
            {
                Vector3 diff = m_points[i - 1] - m_points[i];
                float dist = diff.magnitude;

                if (length + dist >= target || i == m_points.Length - 1)
                {
                    float t = (target - length) / dist;

                    result.Add(Vector3.Lerp(m_points[i - 1], m_points[i], t));

                    target += step;
                }

                length += dist;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Optimized version of position lookup by distance using cache
        /// </summary>
        public Vector3 GetPosByDist(float distance)
        {
            if (m_cachedPointsForLookup == null || m_cachedPointsForLookup.Length == 0)
            {
                return GetPosByDistUncached(distance);
            }

            if (distance <= 0) return m_cachedPointsForLookup[0];
            if (distance >= m_length) return m_cachedPointsForLookup[m_cachedPointsForLookup.Length - 1];

            // Binary search to find closest position in cache
            int left = 0;
            int right = m_cachedDistancesForLookup.Length - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (m_cachedDistancesForLookup[mid] < distance)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            int idx1 = Mathf.Max(0, right);
            int idx2 = Mathf.Min(m_cachedDistancesForLookup.Length - 1, left);

            if (idx1 == idx2) return m_cachedPointsForLookup[idx1];

            // Interpolate between two closest points
            float t = Mathf.InverseLerp(m_cachedDistancesForLookup[idx1], m_cachedDistancesForLookup[idx2], distance);
            return Vector3.Lerp(m_cachedPointsForLookup[idx1], m_cachedPointsForLookup[idx2], t);
        }

        /// <summary>
        /// Original implementation of position lookup by distance (used for building cache)
        /// </summary>
        private Vector3 GetPosByDistUncached(float distance)
        {
            if (m_points.Length == 0)
            {
                Debug.LogError("Way not calculated");
                return Vector3.zero;
            }

            float length = 0f;
            float target = distance;

            Vector3 result = Vector3.zero;

            for (int i = 1; i < m_points.Length; ++i)
            {
                Vector3 diff = m_points[i - 1] - m_points[i];

                float dist = diff.magnitude;

                if (length + dist >= target || i == m_points.Length - 1)
                {
                    float t = (target - length) / dist;

                    result = Vector3.Lerp(m_points[i - 1], m_points[i], t);

                    break;
                }

                length += dist;
            }

            return result;
        }

        public Vector3 GetDirByDist(float distance)
        {
            Vector3 p1 = GetPosByDist(distance - 0.1f);
            Vector3 p2 = GetPosByDist(distance + 0.1f);
            return (p2 - p1).normalized;
        }

        public Vector3 GetPosByFactor(float factor)
        {
            factor = Mathf.Clamp01(factor);
            float distance = GetPathLength() * factor;
            return GetPosByDist(distance);
        }

        public Vector3 GetDirByFactor(float factor)
        {
            factor = Mathf.Clamp01(factor);
            float distance = GetPathLength() * factor;
            return GetDirByDist(distance);
        }

        public Vector3 GetPosByDistWithOffset(float distance, Vector3 offset)
        {
            Vector3 dir = GetDirByDist(distance);
            Vector3 dirOffset = (dir * offset.z) + (new Vector3(dir.z, 0f, -dir.x) * offset.x);
            return GetPosByDist(distance) + dirOffset;
        }

        public Vector3 GetPosByDistWithXOffset(float distance, float offsetX)
        {
            Vector3 dir = GetDirByDist(distance);
            Vector3 dirOffset = new Vector3(dir.z, 0f, -dir.x) * offsetX;
            return GetPosByDist(distance) + dirOffset;
        }

        public Vector3 GetXOffsetByPos(float distance, float offsetX)
        {
            Vector3 dir = GetDirByDist(distance);
            Vector3 dirOffset = new Vector3(dir.z, 0f, -dir.x) * offsetX;
            return dirOffset;
        }

        public float GetOffsetByPosX(float distance, Vector3 pos)
        {
            Vector3 centrePos = GetPosByDist(distance);
            Vector3 dirToPos = pos - centrePos;
            Vector3 posWithOffset = centrePos + GetXOffsetByPos(distance, 1f);
            Vector3 dirWithOffset = posWithOffset - centrePos;
            float angle = Vector3.Angle(dirToPos, dirWithOffset);
            float offsetDir = angle < 90f ? 1 : -1f;
            return dirToPos.magnitude * offsetDir;
        }

        private float CalcLength(List<Vector3> points)
        {
            float len = 0f;
            for (int i = 1; i < points.Count; ++i)
            {
                len += CalculateDistance(points[i - 1], points[i]);
            }
            return len;
        }

        public Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            // The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }

        private void OnDrawGizmos()
        {
            Vector3[] way = GetPoints(m_step);

            // Draw key points
            Gizmos.color = Color.white;
            for (int i = 1; i < transform.childCount - 1; ++i)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, 0.5f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.GetChild(0).position, 0.6f);
            Gizmos.DrawSphere(transform.GetChild(transform.childCount - 1).position, 0.6f);
            Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(1).position);
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(transform.childCount - 2).position);

            // Draw way
            Gizmos.color = Color.yellow;
            for (int i = 0; i < way.Length; i++)
            {
                Gizmos.DrawSphere(way[i], 0.2f);

                if (i < way.Length - 1)
                    Gizmos.DrawLine(way[i], way[i + 1]);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GenWay();

                if (m_rebuildWay) CheckNames();
            }
#endif
        }

        public GameObject[] GetWayPoints()
        {
            GameObject[] go = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                go[i] = transform.GetChild(i).gameObject;
            }

            return go;
        }

        public float GetPathLength()
        {
            return m_length;
        }

        public bool IsWayReady()
        {
            return m_points.Length > 0;
        }

        /// <summary>
        /// Optimized version of finding nearest position on path
        /// </summary>
        public void GetNearestPosOfDist(Vector3 objPosition, out float nearestDist, out bool isFound, float distStep = 1f, float searchDist = 100f)
        {
            if (m_useSpatialPartitioning && m_spatialGrid != null && m_spatialGrid.Count > 0)
            {
                // Use spatial partitioning for fast search
                GetNearestPosOfDistSpatial(objPosition, out nearestDist, out isFound, searchDist);
                return;
            }

            if (m_cachedPointsForLookup != null && m_cachedPointsForLookup.Length > 0)
            {
                // Use cache for fast search
                GetNearestPosOfDistCached(objPosition, out nearestDist, out isFound, searchDist);
                return;
            }

            // Original linear search if cache and spatial partitioning are not available
            float dist = 0f;
            float nearestDistToObj = nearestDist = searchDist;
            isFound = false;

            while (dist < m_length)
            {
                Vector3 distPos = GetPosByDist(dist);
                float distToObj = CalculateDistance(objPosition, distPos);

                if (distToObj < nearestDistToObj)
                {
                    nearestDistToObj = distToObj;
                    nearestDist = dist;
                    isFound = true;
                }

                dist += distStep;
            }
        }

        /// <summary>
        /// Find nearest position using cache
        /// </summary>
        private void GetNearestPosOfDistCached(Vector3 objPosition, out float nearestDist, out bool isFound, float searchDist = 100f)
        {
            nearestDist = 0f;
            isFound = false;

            int bestIndex = -1;
            float bestDistance = searchDist;

            // Find nearest point in cache
            for (int i = 0; i < m_cachedPointsForLookup.Length; i++)
            {
                float dist = CalculateDistance(objPosition, m_cachedPointsForLookup[i]);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    bestIndex = i;
                }
            }

            if (bestIndex >= 0)
            {
                nearestDist = m_cachedDistancesForLookup[bestIndex];
                isFound = true;

                // Refine search in the vicinity of found point
                float rangeStep = 0.1f;
                float searchRange = 0.5f;
                float startDist = Mathf.Max(0, nearestDist - searchRange);
                float endDist = Mathf.Min(m_length, nearestDist + searchRange);

                for (float d = startDist; d <= endDist; d += rangeStep)
                {
                    Vector3 pos = GetPosByDistUncached(d);
                    float dist = CalculateDistance(objPosition, pos);

                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        nearestDist = d;
                    }
                }
            }
        }

        /// <summary>
        /// Find nearest position using spatial partitioning
        /// </summary>
        private void GetNearestPosOfDistSpatial(Vector3 objPosition, out float nearestDist, out bool isFound, float searchDist = 100f)
        {
            nearestDist = 0f;
            isFound = false;

            Vector3Int cell = GetCellForPosition(objPosition);
            float bestDistance = searchDist;
            int bestIndex = -1;

            // Check current cell and neighbors
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        // Skip axes that are ignored in the current distance calculation mode
                        if ((m_distanceMode == DistanceCalculationMode.IgnoreX && x != 0) ||
                            (m_distanceMode == DistanceCalculationMode.IgnoreY && y != 0) ||
                            (m_distanceMode == DistanceCalculationMode.IgnoreZ && z != 0))
                        {
                            continue;
                        }

                        Vector3Int neighborCell = new Vector3Int(
                            cell.x + x,
                            cell.y + y,
                            cell.z + z
                            );

                        if (m_spatialGrid.TryGetValue(neighborCell, out List<int> indices))
                        {
                            foreach (int idx in indices)
                            {
                                float dist = CalculateDistance(objPosition, m_cachedPointsForLookup[idx]);
                                if (dist < bestDistance)
                                {
                                    bestDistance = dist;
                                    bestIndex = idx;
                                }
                            }
                        }
                    }
                }
            }

            if (bestIndex >= 0)
            {
                nearestDist = m_cachedDistancesForLookup[bestIndex];
                isFound = true;

                // Refine search in the vicinity of found point
                float rangeStep = 0.1f;
                float searchRange = 0.5f;
                float startDist = Mathf.Max(0, nearestDist - searchRange);
                float endDist = Mathf.Min(m_length, nearestDist + searchRange);

                for (float d = startDist; d <= endDist; d += rangeStep)
                {
                    Vector3 pos = GetPosByDistUncached(d);
                    float dist = CalculateDistance(objPosition, pos);

                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        nearestDist = d;
                    }
                }
            }
        }

        public void GetClosestPosOfDist(Vector3 objPosition, float dist, float range, out float nearestDist, out bool isFound, float distStep = 0.1f, float searchDist = 100f)
        {
            float nearestDistToObj = nearestDist = searchDist;
            isFound = false;
            float startDist = dist - range;
            float endDist = dist + range;

            if (startDist < 0f) startDist = 0f;
            if (endDist > m_length) endDist = m_length;

            dist = startDist;

            while (dist < endDist)
            {
                Vector3 distPos = GetPosByDist(dist);
                float distToObj = CalculateDistance(objPosition, distPos);

                if (distToObj < nearestDistToObj)
                {
                    nearestDistToObj = distToObj;
                    nearestDist = dist;
                    isFound = true;
                }

                dist += distStep;
            }
        }
    }
}