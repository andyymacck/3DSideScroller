using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic bar that displays icons based on an integer value (e.g., health, ammo).
/// Can be reused for different bar types.
/// </summary>

namespace SideScroller
{
    public class CollectableBar
    {
        private readonly GameObject m_referenceObject;
        private readonly Transform m_parentTransform;
        private readonly float m_offsetItem;
        private readonly Vector2 m_offsetGlobal;

        private readonly List<GameObject> m_spawnedObjects = new List<GameObject>();

        public CollectableBar(GameObject referenceObject, Transform parentTransform, float offsetItem, Vector2 offsetGlobal)
        {
            m_referenceObject = referenceObject;
            m_parentTransform = parentTransform;
            m_offsetItem = offsetItem;
            m_offsetGlobal = offsetGlobal;
        }

        public void UpdateBar(int currentCount, int maxCount)
        {
            ClearBar();

            int clampedCount = Mathf.Clamp(currentCount, 0, maxCount);
            for (int i = 0; i < clampedCount; i++)
            {
                GameObject icon = Object.Instantiate(m_referenceObject, m_parentTransform);
                Vector3 offset = new Vector3(m_offsetItem * i, 0f, 0f);
                Vector3 global = new Vector3(m_offsetGlobal.x, m_offsetGlobal.y, 0f);
                icon.transform.localPosition = global + offset;
                icon.SetActive(true);
                m_spawnedObjects.Add(icon);
            }
        }

        public void ClearBar()
        {
            foreach (var obj in m_spawnedObjects)
            {
                Object.Destroy(obj);
            }
            m_spawnedObjects.Clear();
        }
    }

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject m_referenceObject;
        [SerializeField] private Transform m_transform;
        [SerializeField] private float m_offsetItem;
        [SerializeField] private Vector2 m_offsetGlobal;

        private CollectableBar m_healthIconBar;

        private void Start()
        {
            m_healthIconBar = new CollectableBar(m_referenceObject, m_transform, m_offsetItem, m_offsetGlobal);
            EventHub.Instance.Subscribe<HealthChangeEvent>(UpdateHealth);
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<HealthChangeEvent>(UpdateHealth);
        }

        private void UpdateHealth(HealthChangeEvent eventData)
        {
            m_healthIconBar.UpdateBar(eventData.CurrentHealth, eventData.MaxHealth);
        }
    }
}