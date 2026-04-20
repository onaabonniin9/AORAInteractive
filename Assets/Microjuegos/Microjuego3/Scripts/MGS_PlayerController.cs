using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_PlayerController : MonoBehaviour
    {
        [Header("Estadísticas")]
        public float velocidadNormal = 6f;
        public float velocidadSigilo = 3f;

        [Header("Controles Móviles")]
        public MGS_VirtualJoystick joystick;
        private bool modoSigiloActivo = false; // Se activa desde el botón táctil

        private float velocidadActual;
        private Rigidbody rb;
        private Vector3 inputMovimiento;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            velocidadActual = velocidadNormal;
        }

        void Update()
        {
            float movX = 0f;
            float movZ = 0f;

            // Prioridad al Joystick Táctil. Si no se usa, miramos el teclado (PC).
            if (joystick != null && joystick.InputVector != Vector2.zero)
            {
                movX = joystick.InputVector.x;
                movZ = joystick.InputVector.y;
            }
            else
            {
                movX = Input.GetAxisRaw("Horizontal");
                movZ = Input.GetAxisRaw("Vertical");
            }

            inputMovimiento = new Vector3(movX, 0f, movZ).normalized;

            // Sigilo táctil o barra espaciadora
            if (modoSigiloActivo || Input.GetKey(KeyCode.Space))
            {
                velocidadActual = velocidadSigilo;
                transform.localScale = new Vector3(1f, 0.5f, 1f);
            }
            else
            {
                velocidadActual = velocidadNormal;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + inputMovimiento * velocidadActual * Time.fixedDeltaTime);
        }

        // Estas funciones las llamará el Botón de la UI
        public void ActivarSigilo() { modoSigiloActivo = true; }
        public void DesactivarSigilo() { modoSigiloActivo = false; }
    }
}
