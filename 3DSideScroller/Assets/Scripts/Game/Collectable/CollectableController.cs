using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] PlayerController m_playerController;

        private readonly List<CollectableItem> m_collectedItems = new List<CollectableItem>();

        private GemModel m_GemModel;

        void Start()
        {
            m_playerController.OntriggerEnterEvent += TriggerEnter;

            m_GemModel = new GemModel(CollectabeType.Gem, 0, "");
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
            if (collectableItem.Type == CollectabeType.Gem)
            {
                int count = m_GemModel.Count;
                m_GemModel = new GemModel(CollectabeType.Gem, count + 1, "");

                EventHub.Instance.Publish(new CollectItemEvent(m_GemModel));
            }

            
        }
    }
}