using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_PlayerController : MonoBehaviour
    {
        [Header("Estadísticas")]
        public float velocidadNormal = 6f;
        public float velocidadSigilo = 3f;
        public float velocidadRotacion = 720f;

        [Header("Controles Móviles")]
        public MGS_VirtualJoystick joystick;
        private bool modoSigiloActivo = false;

        [Header("Referencias Animator y Físicas")]
        public Animator animRobot;
        public CapsuleCollider colisionador;

        private float velocidadActual;
        private Rigidbody rb;
        private Vector3 inputMovimiento;
        private Vector3 posicionInicial;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            velocidadActual = velocidadNormal;
            posicionInicial = transform.position;
        }

        void Update()
        {
            // 1. LEER INPUT DEL JOYSTICK
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

            // 2. ROTACIÓN (El modelo mira hacia donde te mueves)
            if (inputMovimiento != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(inputMovimiento, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
            }

            // 3. SISTEMA DE AGACHADO
            if (modoSigiloActivo || Input.GetKey(KeyCode.Space))
            {
                velocidadActual = velocidadSigilo;
                
                if (colisionador != null) 
                {
                    colisionador.height = 1f; // Encoge la cápsula
                    colisionador.center = new Vector3(0f, -0.5f, 0f); // Baja el centro
                }
                
                if (animRobot != null) 
                {
                    animRobot.SetBool("Agachado", true);
                    animRobot.transform.localPosition = new Vector3(0f, -1.0f, 0f); // Sube el modelo para que no atraviese el suelo
                }
            }
            else
            {
                velocidadActual = velocidadNormal;
                
                if (colisionador != null) 
                {
                    colisionador.height = 2f; 
                    colisionador.center = new Vector3(0f, 0f, 0f);
                }
                
                if (animRobot != null) 
                {
                    animRobot.SetBool("Agachado", false);
                    animRobot.transform.localPosition = new Vector3(0f, -1f, 0f); // Posición normal de pie
                }
            }

            // 4. ENVIAR VELOCIDAD A LA ANIMACIÓN
            if (animRobot != null)
            {
                animRobot.SetFloat("Velocidad", inputMovimiento.magnitude);
            }
        }

        void FixedUpdate()
        {
            // Movimiento físico real
            rb.MovePosition(rb.position + inputMovimiento * velocidadActual * Time.fixedDeltaTime);
        }

        // EVENTOS PARA EL BOTÓN DE LA UI
        public void ActivarSigilo() { modoSigiloActivo = true; }
        public void DesactivarSigilo() { modoSigiloActivo = false; }
        
        // ¡NUEVA FUNCIÓN TIPO INTERRUPTOR!
        public void AlternarSigilo()
        {
            modoSigiloActivo = !modoSigiloActivo;
        }

        public void VolverAlInicio()
        {
            transform.position = posicionInicial;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("KillZone") || other.CompareTag("Enemy"))
            {
                VolverAlInicio();
            }
        }
    }
}