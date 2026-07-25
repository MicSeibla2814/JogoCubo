using Unity.Cinemachine;
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

    [Header("Cameras")]
    private CinemachineImpulseSource _impulseSource;//Variavel para referencia


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null) return;
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
            //Destroy(gameObject);
            this.gameObject.SetActive(false);
            _impulseSource.GenerateImpulse();

        
        }
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        movementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        movementInput = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    //Mobile
    public void EsquerdaPress()
    {
        movementInput = new Vector2(-1, 0);
    }

    public void DireitaPress()
    {
        movementInput = new Vector2(1, 0);    
    }

    public void SoltouBotao()
    {
        movementInput = Vector2.zero;
    }
}