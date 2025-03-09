#region Changes
// v. 1.2.0 - delete instance and add flag to way rebuild
#endregion

using System.Collections.Generic;
using Unity.VisualScripting;
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

		private Vector3[] m_points = new Vector3[0];

		private void Awake()
		{
			GenWay();
			//CheckNames();
		}

		public void GenWay()
		{
			Transform root = transform;

			CheckPoints(root);

			float rawLength = 0f;

			for (int i = 1; i < root.childCount - 1; ++i)
			{
				rawLength += Vector3.Distance(root.GetChild(i).position, root.GetChild(i + 1).position);
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
		}

		private void CheckPoints(Transform root)
		{
			if(root.childCount < 3)
			{
				Debug.LogError("NEED MORE POINTS");

				for (int i = 0; i < 3; i++)
				{
					GameObject go = new GameObject("waypoint");
					go.transform.parent = transform;
				}
			}

			//add guide points
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
			if(m_points.Length == 0)
			{
				Debug.LogError("Way not calculated");
			}
		
			float length = 0f;
			float target = 0f;

			int stepsC = (int)(m_length / stepLength);
			float step = m_length / (float)(stepsC+1);

			List<Vector3> result = new List<Vector3>();

			for(int i = 1; i < m_points.Length; ++i)
			{
				Vector3 diff = m_points[i-1] - m_points[i];
				float dist = diff.magnitude;

				if(length + dist >= target || i == m_points.Length - 1)
				{
					float t = (target - length) / dist;

					result.Add(Vector3.Lerp(m_points[i-1], m_points[i], t));

					target+= step;
				}

				length += dist;
			}

			return result.ToArray();
		}

		public Vector3 GetPosByDist(float distance)
		{
			if (m_points.Length == 0)
			{
				Debug.LogError("Way not calculated");
			}

			float length = 0f;
			float target = distance;

			int stepsC = (int)(m_length / m_step);
			float step = m_length / (float)(stepsC + 1);

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

		private float CalcLength( List<Vector3> points )
		{
			float len = 0f;
			for(int i = 1; i < points.Count; ++i)
			{
				Vector3 d = points[i] - points[i-1];
				len += d.magnitude;
			}
			return len;
		}

		public Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
			Vector3 a = 2f * p1;
			Vector3 b = p2 - p0;
			Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
			Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

			//The cubic polynomial: a + b * t + c * t^2 + d * t^3
			Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

			return pos;
		}

		private void OnDrawGizmos()
		{
			Vector3[] way = GetPoints(m_step);

			//draw key points
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

			//draw way
			Gizmos.color = Color.yellow;
			for (int i = 0; i < way.Length; i++)
			{
				Gizmos.DrawSphere(way[i], 0.2f);

				if(i < way.Length - 1)
					Gizmos.DrawLine(way[i], way[i + 1]);
			}

			//Gizmos.color = Color.green;
			//Gizmos.DrawSphere(GetPoint(m_pos), 0.5f);

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				GenWay();
			
				if(m_rebuildWay) CheckNames();
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
	
		/// <summary> return dist of nearest position on the way </summary>
		public void GetNearestPosOfDist(Vector3 objPosition, out float nearestDist, out bool isFound,  float distStep = 1f, float searchDist = 100f)
		{
			float dist = 0f;
			float nearestDistToObj = nearestDist = searchDist;
			isFound = false;

			while (dist < m_length)
			{
				Vector3 distPos = GetPosByDist(dist);
				float distToObj = Vector3.Distance(objPosition, distPos);
			
				if (distToObj < nearestDistToObj)
				{
					nearestDistToObj = distToObj;
					nearestDist = dist;
					isFound = true;
				}
			
				dist += distStep;
			}
		}
		
		public void GetClosestPosOfDist(Vector3 objPosition, float dist, float range, out float nearestDist, out bool isFound,  float distStep = 1f, float searchDist = 100f)
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
				float distToObj = Vector3.Distance(objPosition, distPos);
			
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

