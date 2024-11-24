using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] PlayerController m_playerController;

        private readonly Dictionary<CollectabeType, CollectableModel> m_collectedItems = new Dictionary<CollectabeType, CollectableModel>();

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
                    AddCollectabe(collectableItem);
                    collectableItem.Collect();
                }
            }
        }

        private void AddCollectabe(CollectableItem collectableItem)
        {
            if (m_collectedItems.TryGetValue(collectableItem.Type, out CollectableModel collectable))
            {
                collectable.IncrementCount();
            }
            else
            {
                CollectableModel newItem = CreateModel(collectableItem.Type);

                if(newItem != null)
                {
                    m_collectedItems.Add(collectableItem.Type, newItem);
                }
            }


            EventHub.Instance.Publish(new CollectItemEvent(m_collectedItems));
        }

        private CollectableModel CreateModel(CollectabeType collectabeType)
        {
            switch(collectabeType)
            {
                case CollectabeType.Bomb:
                    return new GemModel(CollectabeType.Bomb, 1, "RR");
                case CollectabeType.Gem:
                    return new GemModel(CollectabeType.Gem, 1, "RR");
                case CollectabeType.Potion:
                    return new GemModel(CollectabeType.Potion, 1, "RR");
                case CollectabeType.Health:
                    return new GemModel(CollectabeType.Health, 1, "RR");
                default:
                    return null;
            }
        }
    }
}