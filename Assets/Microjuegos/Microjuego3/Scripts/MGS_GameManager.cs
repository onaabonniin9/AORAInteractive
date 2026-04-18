using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        public GameObject botonReiniciar; // Arrastraremos el botón aquí

        private float tiempoActual;
        private int monedasRecogidas = 0;
        private bool juegoTerminado = false;

        void Start()
        {
            // Nos aseguramos de que el tiempo corra normal
            Time.timeScale = 1f;
            tiempoActual = tiempoMaximo;
            ActualizarTextoMonedas();

            // Ocultamos el botón de reiniciar al empezar a jugar
            if (botonReiniciar != null) botonReiniciar.SetActive(false);
        }

        void Update()
        {
            if (juegoTerminado) return; // Si ya terminó, no restamos más tiempo

            // Cuenta atrás
            tiempoActual -= Time.deltaTime;

            // Formatear el tiempo a "00:00"
            int minutos = Mathf.FloorToInt(tiempoActual / 60F);
            int segundos = Mathf.FloorToInt(tiempoActual - minutos * 60);
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

            // Comprobar si hemos recogido todas
            if (monedasRecogidas >= totalMonedasNivel)
            {
                GanarJuego();
            }
        }

        private void ActualizarTextoMonedas()
        {
            textoMonedas.text = monedasRecogidas + " / " + totalMonedasNivel;
        }

        public void GanarJuego()
        {
            juegoTerminado = true;
            textoTiempo.text = "¡VICTORIA!";
            textoTiempo.color = Color.green;
            Time.timeScale = 0f; // Congela el juego
            if (botonReiniciar != null) botonReiniciar.SetActive(true); // Muestra el botón
        }

        public void PerderJuego()
        {
            juegoTerminado = true;
            textoTiempo.text = "¡DERROTA!";
            textoTiempo.color = Color.red;
            Time.timeScale = 0f; // Congela el juego
            if (botonReiniciar != null) botonReiniciar.SetActive(true); // Muestra el botón
        }

        public void ReiniciarJuego()
        {
            Time.timeScale = 1f; // ¡MUY IMPORTANTE! Descongelar antes de reiniciar
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
