using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleMovement : MonoBehaviour
{
    public bool quietecitoChill = false;
    public float speed = 10f;
    [HideInInspector] public Transform origin;

    [Header("AR Compensation")]
    public float lateralMultiplier = 3f;

    private UnityEngine.Vector3 initialCameraPos;
    private Camera cam;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Me fallaba tanto que lo forze en codigo... lol
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
        UnityEngine.Vector3 flatForward = origin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        UnityEngine.Vector3 flatRight = origin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        UnityEngine.Vector3 forwardMove = new UnityEngine.Vector3(0,0,0);
        // ===== MOVIMIENTO HACIA JUGADOR =====
        if (!quietecitoChill)
        {
            forwardMove = -flatForward * speed * Time.fixedDeltaTime;
        }

        // ===== OFFSET CÁMARA =====
        UnityEngine.Vector3 cameraOffset = cam.transform.position - initialCameraPos;
        cameraOffset.y = 0;

        float lateral = UnityEngine.Vector3.Dot(cameraOffset, flatRight);

        float exaggeratedLateral = lateral * lateralMultiplier;

        UnityEngine.Vector3 lateralMove =
            -flatRight * exaggeratedLateral * Time.fixedDeltaTime * speed;

        // ===== MOVIMIENTO FINAL =====
        UnityEngine.Vector3 nextPos = rb.position + forwardMove + lateralMove;

        rb.MovePosition(nextPos);

        // ===== CLEANUP =====
        if (UnityEngine.Vector3.Distance(rb.position, origin.position) < 2f)
        {
            Destroy(gameObject);
        }
    }
}