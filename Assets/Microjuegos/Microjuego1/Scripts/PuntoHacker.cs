using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microjuego1
{
    public class PuntosHacker : MonoBehaviour
    {
        [SerializeField] private RectTransform contenedor; // Canvas o panel de fondo
        [SerializeField] private int cantidadPuntos = 30;
        [SerializeField] private float velocidadMin = 40f;
        [SerializeField] private float velocidadMax = 120f;
        [SerializeField] private Color colorPunto = new Color(0f, 1f, 0.8f, 0.4f);

        private List<RectTransform> puntos = new List<RectTransform>();
        private List<float> velocidades = new List<float>();
        private float anchoCanvas;
        private float altoCanvas;

        void Start()
        {
            anchoCanvas = contenedor.rect.width;
            altoCanvas = contenedor.rect.height;

            for (int i = 0; i < cantidadPuntos; i++)
                CrearPunto();
        }

        void Update()
        {
            for (int i = 0; i < puntos.Count; i++)
            {
                // Mueve el punto hacia abajo
                puntos[i].anchoredPosition += Vector2.down * velocidades[i] * Time.deltaTime;

                // Si sale por abajo, lo reinicia arriba en posición aleatoria
                if (puntos[i].anchoredPosition.y < -altoCanvas / 2f)
                    ReiniciarPunto(puntos[i]);
            }
        }

        void CrearPunto()
        {
            // Crea un objeto UI
            GameObject obj = new GameObject("Punto");
            obj.transform.SetParent(contenedor, false);

            // Imagen del punto
            Image img = obj.AddComponent<Image>();
            img.color = colorPunto;

            // Tamaño aleatorio pequeño
            RectTransform rt = obj.GetComponent<RectTransform>();
            float size = Random.Range(2f, 5f);
            rt.sizeDelta = new Vector2(size, size);

            // Posición aleatoria inicial
            rt.anchoredPosition = new Vector2(
                Random.Range(-anchoCanvas / 2f, anchoCanvas / 2f),
                Random.Range(-altoCanvas / 2f, altoCanvas / 2f)
            );

            puntos.Add(rt);
            velocidades.Add(Random.Range(velocidadMin, velocidadMax));
        }

        void ReiniciarPunto(RectTransform rt)
        {
            rt.anchoredPosition = new Vector2(
                Random.Range(-anchoCanvas / 2f, anchoCanvas / 2f),
                altoCanvas / 2f
            );
        }
    }
}