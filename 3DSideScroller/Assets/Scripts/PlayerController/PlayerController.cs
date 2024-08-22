using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_forceMovement = 2f;
    [SerializeField] private float m_forceJump = 2f;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            Move(false);

        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Move(true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

    }

    private void Jump()
    {
        Vector3 moveDir = Vector3.up;
        m_rigidbody.AddForce(moveDir * m_forceJump, ForceMode.Impulse);
    }

    private void Move(bool isLeft)
    {
        Vector3 moveDir = isLeft ? Vector3.left : Vector3.right;
        m_rigidbody.AddForce(moveDir * m_forceMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"[OnTriggerExit] Name:{collision.gameObject.name}, Tag:{collision.gameObject.tag}");
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log($"[OnTriggerExit] Name:{collision.gameObject.name}, Tag:{collision.gameObject.tag}");
    }
}
