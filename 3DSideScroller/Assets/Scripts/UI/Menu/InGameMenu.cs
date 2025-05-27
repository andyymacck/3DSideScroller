using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class InGameMenu : BaseMenu
    {
        [SerializeField] private ProgressBarInt m_healthBar;
        [SerializeField] private ProgressBarInt m_collectables;

        private const int m_maxCollectable = 5;

        void Start()
        {
            EventHub.Instance.Subscribe<HealthChangeEvent>(UpdateHealth);
            EventHub.Instance.Subscribe<CollectItemEvent>(OnCollectItemEvent);
        }

        private void OnDestroy()
        {
            EventHub.Instance.UnSubscribe<HealthChangeEvent>(UpdateHealth);
            EventHub.Instance.UnSubscribe<CollectItemEvent>(OnCollectItemEvent);
        }

        private void UpdateHealth(HealthChangeEvent eventData)
        {
            m_healthBar.UpdateBar(eventData.MaxHealth, eventData.CurrentHealth);
        }

        private void OnCollectItemEvent(CollectItemEvent eventData)
        {
            // Update the view using the single GemModel provided
            UpdateGems(eventData.Models);
        }

        private void UpdateGems(Dictionary<CollectabeType, CollectableModel> Models)
        {
            if (Models.TryGetValue(CollectabeType.Gem, out CollectableModel model))
            {
                m_collectables.UpdateBar(m_maxCollectable, model.Count);
            }
        }
    }
}