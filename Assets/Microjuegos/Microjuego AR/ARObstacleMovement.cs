using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleMovement : MonoBehaviour
{
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public Transform origin;

    [Header("AR Compensation")]
    public float lateralMultiplier = 3f;

    private Vector3 initialCameraPos;
    private Camera cam;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 🔥 clave para triggers fiables
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    private void Start()
    {
        cam = Camera.main;
        if (cam != null)
            initialCameraPos = cam.transform.position;
    }

    private void FixedUpdate()
    {
        if (origin == null || cam == null) return;

        // ===== DIRECCIONES PLANAS =====
        Vector3 flatForward = origin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        Vector3 flatRight = origin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        // ===== MOVIMIENTO HACIA JUGADOR =====
        Vector3 forwardMove = -flatForward * speed * Time.fixedDeltaTime;

        // ===== OFFSET CÁMARA =====
        Vector3 cameraOffset = cam.transform.position - initialCameraPos;
        cameraOffset.y = 0;

        float lateral = Vector3.Dot(cameraOffset, flatRight);

        float exaggeratedLateral = lateral * lateralMultiplier;

        Vector3 lateralMove =
            -flatRight * exaggeratedLateral * Time.fixedDeltaTime * speed;

        // ===== MOVIMIENTO FINAL =====
        Vector3 nextPos = rb.position + forwardMove + lateralMove;

        rb.MovePosition(nextPos);

        // ===== CLEANUP =====
        if (Vector3.Distance(rb.position, origin.position) < 2f)
        {
            Destroy(gameObject);
        }
    }
}