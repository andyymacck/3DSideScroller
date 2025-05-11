using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Exctract bar functionality to the separate extarnal class. That can be reused for any bar in the game
    // This kind of bar using int value to spawn the objects.
    // You need to create a float bar with the image fill for the same purpose.

    // When it is done. Move sunbcribe to the InGameMenu
    [SerializeField] private GameObject m_referenceObject; // Prefab to instantiate
    [SerializeField] private Transform m_transform; // Parent transform for spawned objects
    [SerializeField] private float m_offsetItem; // Offset between spawned objects
    [SerializeField] private Vector2 m_offsetGlobal; // Offset for all spawned objects
    [SerializeField] private int m_countMax = 3; // Max visible health count

    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    private void Start()
    {
        EventHub.Instance.Subscribe<HealthChangeEvent>(UpdateHealth);
    }

    private void OnDestroy()
    {
        EventHub.Instance.UnSubscribe<HealthChangeEvent>(UpdateHealth);
    }

    // "update view"
    private void UpdateHealth(HealthChangeEvent eventData)
    {
        ClearView();

        m_countMax = eventData.MaxHealth;

        int count = Mathf.Clamp(eventData.CurrentHealth, 0, m_countMax);

        for (int i = 0; i < count; i++)
        {
            GameObject newHealthIcon = Instantiate(m_referenceObject, m_transform);
            Vector3 offset = new Vector3(m_offsetItem * i, 0f, 0f);
            Vector3 global = new Vector3(m_offsetGlobal.x, m_offsetGlobal.y, 0f);
            newHealthIcon.transform.localPosition = global + offset;
            newHealthIcon.SetActive(true);
            m_spawnedObjects.Add(newHealthIcon);
        }
    }

    private void ClearView()
    {
        foreach (GameObject collectableObject in m_spawnedObjects)
        {
            Destroy(collectableObject);
        }

        m_spawnedObjects.Clear();
    }
}