using SideScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ENEMY_TAG_ID))
        {
            // add damage to the weapon and use this value to deal damage to the enemy
            // update enemy TAG to the "enemy'
            // change matrix that weapon shoyl collide only with enemies

            Debug.Log("WEAPON HIT");

            Unit unit = other.GetComponent<Unit>();
            unit.DealDamage(25);
        }
    }
}
