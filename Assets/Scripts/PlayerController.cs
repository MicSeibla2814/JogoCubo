using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector2 movementInput;

    [Header("Particulas")]
    public ParticleSystem particleDestruction;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movimentacao();
    }
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void Movimentacao()
    {
        if (rb.linearVelocity.magnitude < maxSpeed)
        {

            Vector3 moveDirection = new Vector3
                (movementInput.x, 0, movementInput.y) * moveSpeed;

            rb.linearVelocity = new Vector3
                (moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(
                particleDestruction, transform.position, Quaternion.identity);
            GameManager.Instance.GameOver();
            Destroy(gameObject);
       
        }
    }
}