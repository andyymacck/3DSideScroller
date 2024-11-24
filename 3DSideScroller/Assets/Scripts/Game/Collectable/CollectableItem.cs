using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private CollectabeType m_type;
    //if you want ot use color you can use color code as sring

    public CollectabeType Type => m_type;


    public void Collect()
    {
        Destroy(gameObject);
    }
}
