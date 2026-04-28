using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        if (transform.position.y < 1.5f)
        {
            Respawn();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX * speed, rb.linearVelocity.y, moveZ * speed);
        rb.linearVelocity = movement;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
        isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || 
            other.CompareTag("Enemy") || 
            other.CompareTag("KillZone"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isGrounded = false;
    }
}