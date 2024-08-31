using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    [SerializeField] private Collider m_collider;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeColissionState(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeColissionState(other, false);
        }
    }

    private void ChangeColissionState(Collider other, bool isEnable)
    {
        Physics.IgnoreCollision(other, m_collider, isEnable);
    }
}