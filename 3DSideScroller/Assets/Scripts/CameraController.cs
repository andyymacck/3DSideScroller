using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private Transform m_transform;
    [SerializeField] private Vector3 m_cameraOffset;
    [SerializeField] private Vector3 m_cameraOffsetStart;
    [SerializeField] private float m_speedVertical = 1f;
    [SerializeField] private float m_speedHorizontal = 1f;
    [SerializeField] private float m_playerSpeedOffsetFactor = 1f;


    private void Awake()
    {
        m_transform.position = m_player.transform.position + m_cameraOffsetStart;
    }

    void Start()
    {
        
    }
 
    void Update()
    {
        Vector3 distance = m_player.transform.position - m_transform.position;
        Vector3 targetPos = m_player.transform.position + m_cameraOffset;
        float targetX = targetPos.x + (m_player.PlayerVelocity.x * m_playerSpeedOffsetFactor);

        float x = Mathf.Lerp(m_transform.transform.position.x, targetX, Time.deltaTime * m_speedHorizontal);
        float y = Mathf.Lerp(m_transform.transform.position.y, targetPos.y, Time.deltaTime * m_speedVertical);
        float z = Mathf.Lerp(m_transform.transform.position.z, targetPos.z, Time.deltaTime);
        m_transform.position = new Vector3(x, y, z);
    }
}