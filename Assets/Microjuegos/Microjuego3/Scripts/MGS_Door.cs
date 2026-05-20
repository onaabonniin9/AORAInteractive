using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_Door : MonoBehaviour
    {
        public float velocidadApertura = 5f;
        private bool abriendo = false;
        private Vector3 posicionObjetivo;

        void Start()
        {
            posicionObjetivo = transform.position + Vector3.down * 4f;
        }

        void Update()
        {
            if (abriendo)
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadApertura * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, posicionObjetivo) < 0.01f)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void AbrirPuerta()
        {
            Debug.Log("¡9 monedas recogidas! Abriendo acceso a la moneda final...");
            abriendo = true;
        }
    }
}