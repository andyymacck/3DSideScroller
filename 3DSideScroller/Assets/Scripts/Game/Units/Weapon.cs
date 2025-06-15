using SideScroller;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int m_damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && unit.IsAlive)
        {
            Debug.Log("WEAPON HIT: " + other.gameObject.name);
            unit.DealDamage(m_damage);
        }
    }
}

