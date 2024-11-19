// CollectableView.cs
using System.Collections.Generic;
using UnityEngine;

public class CollectableView : MonoBehaviour
{
    public CollectableModel m_model;

    [SerializeField] private GameObject m_referenceObject;
    [SerializeField] private Transform m_transform;
    [SerializeField] private float m_offset;

    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    private void Start()
    {
        EventHub.Instance.Subcribe<CollectItemEvent>(UpdateView);
    }

    private void OnDestroy()
    {
        EventHub.Instance.UnSubcribe<CollectItemEvent>(UpdateView);
    }

    private void UpdateView(CollectItemEvent eventData)
    {
        DrawView(eventData.GemModel);
    }

    private void DrawView(GemModel gemModel)
    {
        // FIX THIS ****
        // MAKE SPAWN CORRECT
        // TRY TO PASS A COLLETION OF THE MODELS INSTED ON ONLY 1 MODEL - REFERENCE IS A EVENT HUB SUBC and UNSUBCRRIBE

        for (int i = 0; i < m_spawnedObjects.Count; i++)
        {
            //Destroy(m_spawnedObjects[i]);
            //m_spawnedObjects.RemoveAt(i);
        }

        foreach(var obj in m_spawnedObjects)
        {
            Destroy(obj);
        }

        for (int i = 0; i < gemModel.Count; i++)
        {
            m_spawnedObjects.Add(m_referenceObject);
            GameObject go = Instantiate(m_referenceObject, m_transform);
            go.transform.position = m_transform.position + m_offset * Vector3.right;
            go.SetActive(true);
        }
    }



    public void SetModel(CollectableModel model)
    {
        m_model = model;
        UpdateView();
    }

    void UpdateView()
    {
        // Example of changing the appearance based on type
        //Debug.Log("Displaying: " + m_model.CollectableName);
        // Use different effects or animations here depending on the item type
    }
}
