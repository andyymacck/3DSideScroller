using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Transform m_transform;
    [SerializeField] private Vector3 m_rotateSpeed;


    void Update()
    {
        Vector3 speed = m_rotateSpeed * Time.deltaTime;
        m_transform.Rotate(speed);
    }
}