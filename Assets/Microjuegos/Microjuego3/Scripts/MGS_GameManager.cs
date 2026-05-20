using UnityEngine;
using TMPro;

namespace Microjuego3_MGS
{
    public class MGS_GameManager : MonoBehaviour
    {
        [Header("Configuración")]
        public float tiempoMaximo = 60f; // Segundos que tiene el jugador
        public int totalMonedasNivel = 10;

        [Header("Interfaz (UI)")]
        public TextMeshProUGUI textoTiempo;
        public TextMeshProUGUI textoMonedas;

        private float tiempoActual;
        private int monedasRecogidas = 0;
        private bool juegoTerminado = false;

        void Start()
        {
            // Nos aseguramos de que el tiempo corra normal
            Time.timeScale = 1f;
            tiempoActual = tiempoMaximo;
            ActualizarTextoMonedas();
        }

        void Update()
        {
            if (juegoTerminado) return; // Si ya terminó, no restamos más tiempo

            // Cuenta atrás
            tiempoActual -= Time.deltaTime;

            // Formatear el tiempo a "00:00"
            int minutos = Mathf.FloorToInt(tiempoActual / 60F);
            int segundos = Mathf.FloorToInt(tiempoActual - minutos * 60);
            
            if (textoTiempo != null)
                textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Perder por tiempo
            if (tiempoActual <= 0)
            {
                tiempoActual = 0;
                PerderJuego();
            }
        }

        // Esta función la llamará la moneda cuando la toquemos
        public void SumarMoneda()
        {
            if (juegoTerminado) return;

            monedasRecogidas++;
            ActualizarTextoMonedas();

            // Si tenemos exactamente 9 monedas, buscamos la puerta y la abrimos
            if (monedasRecogidas == 9)
            {
                MGS_Door puerta = Object.FindFirstObjectByType<MGS_Door>();
                if (puerta != null)
                {
                    puerta.AbrirPuerta();
                }
            }

            // Comprobar si hemos recogido todas
            if (monedasRecogidas >= totalMonedasNivel)
            {
                GanarJuego();
            }
        }

        private void ActualizarTextoMonedas()
        {
            if (textoMonedas != null)
                textoMonedas.text = monedasRecogidas + " / " + totalMonedasNivel;
        }

        public void GanarJuego()
        {
            if (juegoTerminado) return;
            juegoTerminado = true;

            if (textoTiempo != null)
            {
                textoTiempo.text = "¡VICTORIA!";
                textoTiempo.color = Color.green;
            }

            Time.timeScale = 0f; // Congela el juego

            if (GlobalGameManager.instance != null)
            {
                GlobalGameManager.instance.WinLevel(monedasRecogidas);
            }
            else
            {
                Debug.LogWarning("GlobalGameManager no detectado.");
            }
        }

        public void PerderJuego()
        {
            if (juegoTerminado) return;
            juegoTerminado = true;

            if (textoTiempo != null)
            {
                textoTiempo.text = "¡DERROTA!";
                textoTiempo.color = Color.red;
            }

            Time.timeScale = 0f; // Congela el juego

            if (GlobalGameManager.instance != null)
            {
                GlobalGameManager.instance.LoseLevel();
            }
            else
            {
                Debug.LogWarning("GlobalGameManager no detectado.");
            }
        }
    }
}