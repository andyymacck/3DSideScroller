using System;
using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] PlayerController m_playerController;

        private readonly Dictionary<CollectabeType, CollectableModel> m_collectedItems = new Dictionary<CollectabeType, CollectableModel>();

        /// <summary>
        /// Add Save and Load to save all collectable data. You can use json to save and load m_collectedItems
        /// </summary>

        void Start()
        {
            m_playerController.OntriggerEnterEvent += TriggerEnter;
        }


        void Update()
        {

        }

        private void OnDestroy()
        {
            m_playerController.OntriggerEnterEvent -= TriggerEnter;
        }

        private void TriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.COLLECTABLE_TAG_ID))
            {
                CollectableItem collectableItem = other.GetComponent<CollectableItem>();
                
                if (collectableItem != null)
                {
                    if(collectableItem.IsRequiredCallback)
                    {
                        Action<CollectableItem> callback = (collectableItem) => Collect(collectableItem);
                        EventHub.Instance.Publish(new CollectItemApprovalEvent(callback, collectableItem));
                    }
                    else
                    {
                        Collect(collectableItem);
                    }
                }
            }
        }

        private void Collect(CollectableItem collectableItem)
        {
            AddCollectabe(collectableItem);
            collectableItem.Collect();
        }

        private void AddCollectabe(CollectableItem collectableItem)
        {
            if (m_collectedItems.TryGetValue(collectableItem.Collectable.CollectabeType, out CollectableModel collectable))
            {
                collectable.IncrementCount();
            }
            else
            {
                CollectableModel newItem = CreateModel(collectableItem.Collectable.CollectabeType);

                if(newItem != null)
                {
                    m_collectedItems.Add(collectableItem.Collectable.CollectabeType, newItem);
                }
            }

            EventHub.Instance.Publish(new CollectItemEvent(m_collectedItems));
        }

        private CollectableModel CreateModel(CollectabeType collectabeType)
        {
            switch(collectabeType)
            {
                case CollectabeType.Coin:
                    return new CoinModel(CollectabeType.Coin, 1);
                case CollectabeType.Gem:
                    return new GemModel(CollectabeType.Gem, 1, "RR");
                case CollectabeType.Potion:
                    return new PotionModel(CollectabeType.Potion, 1, "RR");
                case CollectabeType.Health:
                    return new HealthModel(CollectabeType.Health, 1);
                default:
                    return null;
            }
        }
    }
}