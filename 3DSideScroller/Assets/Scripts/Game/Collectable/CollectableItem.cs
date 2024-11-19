using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private CollectabeType m_type;

    public CollectabeType Type => m_type;


    public void Collect()
    {
        Destroy(gameObject);
    }
}
