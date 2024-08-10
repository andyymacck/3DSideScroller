using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_speedMovement = 2f;
    [SerializeField] private float m_speedJump = 2f;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }

    private void Jump()
    {
        Vector3 moveDir = Vector3.up;
        m_rigidbody.AddForce(moveDir * m_speedJump);
    }


    private void Move(bool isLeft)
    {
        Vector3 moveDir = isLeft ? Vector3.left : Vector3.right;
        m_rigidbody.AddForce(moveDir * m_speedMovement);
    }
}
