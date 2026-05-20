using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_Coin : MonoBehaviour
    {
        [Header("Configuración de la Moneda")]
        public float velocidadRotacion = 100f;
        
        [Header("Sonido")]
        public AudioClip sonidoRecoger;

        private MGS_GameManager gameManager;

        void Start()
        {
            // Busca al Game Manager automáticamente al empezar
            gameManager = Object.FindFirstObjectByType<MGS_GameManager>();
        }

        void Update()
        {
            // Hace que la moneda gire sobre sí misma (estilo juego retro)
            transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.Self);
        }

        void OnTriggerEnter(Collider other)
        {
            // Si lo que choca contra la moneda es el Jugador...
            if (other.CompareTag("Player"))
            {
                if (gameManager != null)
                {
                    gameManager.SumarMoneda();
                }

                if (sonidoRecoger != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
                }

                // ¡Destruye la moneda para que desaparezca de la pantalla!
                Destroy(gameObject);
            }
        }
    }
}