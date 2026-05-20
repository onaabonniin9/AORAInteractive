using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_CameraFollow : MonoBehaviour
    {
        [Header("Configuración")]
        public Transform target;     
        
        [Tooltip("Posición relativa de la cámara respecto al jugador (X, Y, Z)")]
        public Vector3 offset = new Vector3(0f, 12f, -10f); 
        
        public float suavizado = 5f;  

        void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // La cámara persigue al jugador manteniendo la distancia (offset)
            Vector3 posicionDestino = target.position + offset;
            
            // Movimiento fluido
            transform.position = Vector3.Lerp(transform.position, posicionDestino, suavizado * Time.deltaTime);
        }
    }
}