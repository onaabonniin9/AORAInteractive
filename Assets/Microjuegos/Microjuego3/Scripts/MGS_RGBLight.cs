using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_RGBLight : MonoBehaviour
    {
        private Light miLuz;
        
        [Header("Configuración RGB")]
        public bool cicloColorCompleto = true; // Si cambia por todo el arcoíris
        public float velocidad = 0.5f;          // Velocidad del cambio
        public float intensidadMin = 1.5f;      // Pulso de intensidad
        public float intensidadMax = 2.5f;

        void Start()
        {
            miLuz = GetComponent<Light>();
        }

        void Update()
        {
            if (miLuz == null) return;

            // 1. Efecto de Ciclo de Color (Arcoíris)
            if (cicloColorCompleto)
            {
                // Usamos el tiempo para movernos por el espectro de color (Hue)
                float h = Mathf.PingPong(Time.time * velocidad, 1f); 
                miLuz.color = Color.HSVToRGB(h, 0.8f, 1f); 
            }

            // 2. Efecto de Pulso (Respiración)
            // Esto hace que la luz brille más y menos suavemente
            miLuz.intensity = Mathf.Lerp(intensidadMin, intensidadMax, Mathf.PingPong(Time.time * 1.5f, 1f));
        }
    }
}