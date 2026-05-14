using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float rotationSpeed = 10f;

    public MGS_VirtualJoystick joystick;

    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 spawnPoint;

    private Animator animator;

    private PlayerAudio playerAudio;

    private bool gameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPoint = transform.position;

        animator = GetComponentInChildren<Animator>();

        playerAudio = GetComponent<PlayerAudio>();

        StartCoroutine(EnableGameplay());
    }

    IEnumerator EnableGameplay()
    {
        yield return new WaitForSeconds(0.5f);
        gameStarted = true;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();

        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        if (transform.position.y < 1.5f)
        {
            Respawn();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (joystick != null)
        {
            moveX += joystick.InputDirection.x;
            moveZ += joystick.InputDirection.z;
        }

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);

        Vector3 movement = new Vector3(
            moveDirection.x * speed,
            rb.linearVelocity.y,
            moveDirection.z * speed
        );

        rb.linearVelocity = movement;

        bool isMoving = moveDirection.magnitude > 0.01f && isGrounded;

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }

        if (isMoving && moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    public void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;

        if (animator != null)
        {
            animator.SetTrigger("Jump");
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
        if (gameStarted && playerAudio != null)
        {
            playerAudio.PlayHurtSound();
        }

        transform.position = spawnPoint;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isGrounded = false;
    }
}