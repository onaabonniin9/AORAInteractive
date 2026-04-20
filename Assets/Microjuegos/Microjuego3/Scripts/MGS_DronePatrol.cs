using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_DronePatrol : MonoBehaviour
    {
        [Header("Configuración de Patrulla")]
        public Transform[] puntosDePatrulla;
        public float velocidad = 3f;
        public float velocidadRotacion = 5f;

        [Header("Jugador (Para el reinicio)")]
        public Transform puntoInicioJugador;

        private int indiceActual = 0;
        private Vector3 posicionInicialJugador;

        void Start()
        {
            // Guardamos la posición inicial del jugador para devolverlo ahí
            if (puntoInicioJugador != null)
            {
                posicionInicialJugador = puntoInicioJugador.position;
            }
        }

        void Update()
        {
            if (puntosDePatrulla.Length == 0) return;

            Transform destino = puntosDePatrulla[indiceActual];

            // Movimiento hacia el punto
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

            // Rotación hacia el punto
            Vector3 direccion = (destino.position - transform.position).normalized;
            if (direccion != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
            }

            // Cambiar al siguiente punto
            if (Vector3.Distance(transform.position, destino.position) < 0.2f)
            {
                indiceActual = (indiceActual + 1) % puntosDePatrulla.Length;
            }
        }

        // Detección con el láser (VisionCone)
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("¡Jugador detectado! Volviendo al inicio...");

                if (puntoInicioJugador != null)
                {
                    other.transform.position = posicionInicialJugador;
                }
            }
        }
    }
}
