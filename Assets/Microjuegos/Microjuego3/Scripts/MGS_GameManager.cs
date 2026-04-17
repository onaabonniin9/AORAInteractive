using UnityEngine;
using TMPro; // Necesario para la UI moderna de Unity

namespace Microjuego3_MGS
{
    public class MGS_GameManager : MonoBehaviour
    {
        [Header("Configuración del Nivel")]
        public int monedasNecesarias = 10;
        public float tiempoRestante = 180f; // 3 minutos

        [Header("Interfaz de Usuario")]
        public TextMeshProUGUI textoTiempo;
        public TextMeshProUGUI textoMonedas;

        private int monedasActuales = 0;
        private bool juegoTerminado = false;

        void Update()
        {
            if (juegoTerminado) return;

            // Restar tiempo
            tiempoRestante -= Time.deltaTime;
            ActualizarUI();

            // Condición de Derrota por tiempo
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                PerderJuego();
            }
        }

        public void RecogerMoneda()
        {
            if (juegoTerminado) return;

            monedasActuales++;
            ActualizarUI();

            // Condición de Victoria
            if (monedasActuales >= monedasNecesarias)
            {
                GanarJuego();
            }
        }

        public void ReiniciarJuego()
        {
            // Esto recarga la escena en la que estás actualmente
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void ActualizarUI()
        {
            if (textoTiempo != null)
            {
                int minutos = Mathf.FloorToInt(tiempoRestante / 60);
                int segundos = Mathf.FloorToInt(tiempoRestante % 60);
                textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            }
            if (textoMonedas != null)
            {
                textoMonedas.text = monedasActuales + " / " + monedasNecesarias;
            }
        }

        void GanarJuego()
        {
            juegoTerminado = true;
            textoTiempo.text = "¡VICTORIA!";
            textoTiempo.color = Color.green;
            Debug.Log("¡Has recogido todas las monedas y hackeado el servidor!");
            // Aquí en el futuro enlazaremos con el menú de tu compañera
        }

        void PerderJuego()
        {
            juegoTerminado = true;
            textoTiempo.text = "¡DERROTA!";
            textoTiempo.color = Color.red;
            Debug.Log("Se acabó el tiempo.");
        }
    }
}
