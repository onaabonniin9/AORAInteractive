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
        private bool modoSigiloActivo = false;

        private float velocidadActual;
        private Rigidbody rb;
        private Vector3 inputMovimiento;
        private Vector3 posicionInicial; // Aquí guardamos dónde empieza el nivel

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            velocidadActual = velocidadNormal;
            posicionInicial = transform.position; // Guarda el (0, 1, 0) o donde lo pongas al inicio
        }

        void Update()
        {
            float movX = 0f;
            float movZ = 0f;

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

            // SISTEMA DE AGACHADO (Reduce la escala Y a la mitad)
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

        // Esta función la llamaremos desde el Botón de la UI
        public void ToggleCrouch() 
        { 
            modoSigiloActivo = !modoSigiloActivo; 
        }

        public void VolverAlInicio()
        {
            transform.position = posicionInicial;
        }

        // Si choca con un láser (KillZone) o Dron (Enemy), vuelve al inicio
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("KillZone") || other.CompareTag("Enemy"))
            {
                VolverAlInicio();
            }
        }
    }
}