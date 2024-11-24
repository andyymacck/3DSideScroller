using System.Collections.Generic;
using UnityEngine;

public class CollectableView : MonoBehaviour
{
    [SerializeField] private CollectabeType m_viewType;

    [SerializeField] private GameObject m_referenceObject; // Prefab to instantiate
    [SerializeField] private Transform m_transform; // Parent transform for spawned objects
    [SerializeField] private float m_offsetItem; // Offset between spawned objects
    [SerializeField] private float m_offsetGlobal; // Offset for all spawned objects

    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    private void Start()
    {
        // Subscribe to the EventHub to react to single GemModel updates
        EventHub.Instance.Subscribe<CollectItemEvent>(OnCollectItemEvent);
    }

    private void OnDestroy()
    {
        // Unsubscribe when the object is destroyed
        EventHub.Instance.UnSubscribe<CollectItemEvent>(OnCollectItemEvent);
    }

    private void OnCollectItemEvent(CollectItemEvent eventData)
    {
        // Update the view using the single GemModel provided
        UpdateView(eventData.Models);
    }

    private void UpdateView(Dictionary<CollectabeType, CollectableModel> Models)
    {
        if (Models.TryGetValue(m_viewType, out CollectableModel model))
        {
            SpawnObjectsInTheView(model);
        }
        else
        {
            ClearView();
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

    private void SpawnObjectsInTheView(CollectableModel collectableModel)
    {
        ClearView();

        int count = collectableModel.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject newCollectable = Instantiate(m_referenceObject, m_transform);
            Vector3 offset = new Vector3(m_offsetItem * i, 0f, 0f);
            Vector3 global = new Vector3(m_offsetGlobal, 0f, 0f);
            newCollectable.transform.localPosition = m_transform.position + global + offset;
            newCollectable.SetActive(true);
            m_spawnedObjects.Add(newCollectable);
        }
    }
}