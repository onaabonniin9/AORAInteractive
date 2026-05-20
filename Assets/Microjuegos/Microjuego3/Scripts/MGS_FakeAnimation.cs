using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_FakeAnimation : MonoBehaviour
    {
        [Header("Configuración Visual")]
        public Transform modeloSnake; // Arrastra aquí el modelo de Snake
        
        [Header("Ajustes de Bamboleo (Caminar)")]
        public float velocidadPasos = 14f;  // Qué tan rápido "pisa"
        public float cantidadBobbing = 0.05f; // Cuánto sube y baja
        public float cantidadBalanceo = 2f;    // Cuánto se inclina a los lados

        private Vector3 posicionOriginalModel;
        private Rigidbody rb;
        private float timer = 0f;

        void Start()
        {
            rb = GetComponent<Rigidbody>(); // Obtenemos el RB del padre (MGS_Player)
            
            if (modeloSnake != null)
            {
                posicionOriginalModel = modeloSnake.localPosition;
            }
            else
            {
                Debug.LogError("Rafa, te falta arrastrar el modelo de Snake al script en el Inspector!");
            }
        }

        void Update()
        {
            if (modeloSnake == null || rb == null) return;

            // Calculamos la velocidad en el suelo (ignorando la Y)
            float velocidadSuelo = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

            // Si se está moviendo (umbral bajo), aplicamos bamboleo
            if (velocidadSuelo > 0.1f)
            {
                timer += Time.deltaTime * velocidadPasos;

                // 1. Bobbing (Subir y bajar)
                float offset_Y = Mathf.Abs(Mathf.Sin(timer)) * cantidadBobbing;
                modeloSnake.localPosition = posicionOriginalModel + new Vector3(0f, offset_Y, 0f);

                // 2. Swaying (Inclinarse a los lados)
                float inclinacion_Z = Mathf.Sin(timer) * cantidadBalanceo;
                modeloSnake.localRotation = Quaternion.Euler(0f, 0f, inclinacion_Z);
            }
            else
            {
                // Si está quieto, reseteamos posición y rotación suavemente
                timer = 0f;
                modeloSnake.localPosition = Vector3.Lerp(modeloSnake.localPosition, posicionOriginalModel, Time.deltaTime * 5f);
                modeloSnake.localRotation = Quaternion.Lerp(modeloSnake.localRotation, Quaternion.identity, Time.deltaTime * 5f);
            }
        }
    }
}