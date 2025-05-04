using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private bool m_isRequiredCallback = false;
    [SerializeField] private CollectableModel m_collectable;

    public CollectableModel Collectable => m_collectable;
    public bool IsRequiredCallback => m_isRequiredCallback;


    public void Collect()
    {
        Destroy(gameObject);
    }
}
