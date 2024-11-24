using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableNumericView : MonoBehaviour
{
    [SerializeField] private CollectabeType m_viewType;
    [SerializeField] private TMP_Text m_itemAmount;

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the EventHub to react to single GemModel updates
        EventHub.Instance.Subscribe<CollectItemEvent>(OnCollectItemEvent);
        SetText(0);
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
            SetText(model.Count);
        }
        else
        {
            SetText(0);
        }
    }

    private void SetText(int amount)
    {
        m_itemAmount.text = $"x{amount}";
    }
}