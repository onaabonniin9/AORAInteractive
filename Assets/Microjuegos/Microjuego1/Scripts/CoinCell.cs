using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// CoinCell - Representa cada casilla del grid 3x3
/// Gestiona sus estados visuales: idle, mostrando moneda, correcto, incorrecto
/// Optimizado para touch en iOS con IPointerClickHandler
/// </summary>

namespace Microjuego1
{
    public class CoinCell : MonoBehaviour, IPointerClickHandler
    {
        [Header("Elementos visuales")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image coinIcon;        // Sprite de la moneda/€
        [SerializeField] private Image feedbackIcon;    // Checkmark o X
        [SerializeField] private GameObject glitchEffect; // Opcional: partícula glitch

        [Header("Colores")]
        [SerializeField] private Color colorIdle = new Color(0.07f, 0.1f, 0.15f);
        [SerializeField] private Color colorCoin = new Color(0.06f, 0.2f, 0.13f);
        [SerializeField] private Color colorCorrect = new Color(0.06f, 0.25f, 0.13f);
        [SerializeField] private Color colorWrong = new Color(0.18f, 0.06f, 0.07f);
        [SerializeField] private Color colorBorderIdle = new Color(0f, 1f, 0.8f, 0.13f);
        [SerializeField] private Color colorBorderCoin = new Color(0f, 1f, 0.8f, 1f);
        [SerializeField] private Color colorBorderCorrect = new Color(0f, 1f, 0.53f, 1f);
        [SerializeField] private Color colorBorderWrong = new Color(1f, 0.27f, 0.27f, 1f);

        [Header("Animación")]
        [SerializeField] private float appearDuration = 0.25f;
        [SerializeField] private float disappearDuration = 0.2f;

        [Header("Grid Index")]
        [SerializeField] private int cellIndex;

        public bool IsSelected { get; private set; } = false;

        private GridController gridController;
        private Outline borderOutline;

        void Start()
        {
            GameObject gc = GameObject.Find("GridController");
            if (gc != null)
                gridController = gc.GetComponent<GridController>();

            if (gridController == null)
                Debug.Log("SIGUE NULL en celda: " + cellIndex);
            else
                Debug.Log("GridController encontrado en celda: " + cellIndex);

            borderOutline = GetComponent<Outline>();
            if (borderOutline == null)
                borderOutline = gameObject.AddComponent<Outline>();
        }

        public void ResetCell()
        {
            IsSelected = false;
            coinIcon.gameObject.SetActive(false);
            feedbackIcon.gameObject.SetActive(false);
            if (glitchEffect) glitchEffect.SetActive(false);
            SetBackground(colorIdle, colorBorderIdle);
            transform.localScale = Vector3.one;
        }

        public void ShowCoin()
        {
            SetBackground(colorCoin, colorBorderCoin);
            coinIcon.gameObject.SetActive(true);
            feedbackIcon.gameObject.SetActive(false);
            StartCoroutine(ScaleIn(coinIcon.transform));
        }

        public void HideCoin()
        {
            StartCoroutine(FadeOutCoin());
        }

        public void ShowCorrect()
        {
            SetBackground(colorCorrect, colorBorderCorrect);
            coinIcon.gameObject.SetActive(true);
            feedbackIcon.gameObject.SetActive(false);
            StartCoroutine(PulseScale(transform, 1.08f, 0.15f));
        }

        public void ShowWrong()
        {
            SetBackground(colorWrong, colorBorderWrong);
            coinIcon.gameObject.SetActive(false);
            StartCoroutine(GlitchConX());
        }

        IEnumerator GlitchConX()
        {
            for (int i = 0; i < 6; i++)
            {
                feedbackIcon.gameObject.SetActive(true);
                SetBackground(colorWrong, colorBorderWrong);
                yield return new WaitForSeconds(0.06f);

                feedbackIcon.gameObject.SetActive(false);
                SetBackground(colorIdle, colorBorderIdle);
                yield return new WaitForSeconds(0.06f);
            }

            feedbackIcon.gameObject.SetActive(true);
            SetBackground(colorWrong, colorBorderWrong);
            yield return new WaitForSeconds(0.4f);

            feedbackIcon.gameObject.SetActive(false);
            SetBackground(colorIdle, colorBorderIdle);
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
        }

        // Toque en iOS/PC
        public void OnPointerClick(PointerEventData eventData)
        {
            if (gridController != null)
                gridController.OnCellTapped(cellIndex);
        }

        public void OnCellClicked()
        {
            if (gridController != null)
                gridController.OnCellTapped(cellIndex);
            else
                Debug.Log("GridController es NULL");
        }

        // -------- Animaciones --------

        private void SetBackground(Color bg, Color border)
        {
            backgroundImage.color = bg;
            if (borderOutline != null)
                borderOutline.effectColor = border;
        }

        private IEnumerator ScaleIn(Transform t)
        {
            t.localScale = Vector3.zero;
            float elapsed = 0f;
            while (elapsed < appearDuration)
            {
                elapsed += Time.deltaTime;
                float s = Mathf.SmoothStep(0f, 1f, elapsed / appearDuration);
                t.localScale = Vector3.one * s;
                yield return null;
            }
            t.localScale = Vector3.one;
        }

        private IEnumerator FadeOutCoin()
        {
            float elapsed = 0f;
            Color startColor = coinIcon.color;
            while (elapsed < disappearDuration)
            {
                elapsed += Time.deltaTime;
                float a = Mathf.Lerp(1f, 0f, elapsed / disappearDuration);
                coinIcon.color = new Color(startColor.r, startColor.g, startColor.b, a);
                yield return null;
            }
            coinIcon.gameObject.SetActive(false);
            coinIcon.color = startColor;
            SetBackground(colorIdle, colorBorderIdle);
        }

        private IEnumerator PulseScale(Transform t, float maxScale, float duration)
        {
            float half = duration / 2f;
            float e = 0f;
            while (e < half) { e += Time.deltaTime; t.localScale = Vector3.one * Mathf.Lerp(1f, maxScale, e / half); yield return null; }
            e = 0f;
            while (e < half) { e += Time.deltaTime; t.localScale = Vector3.one * Mathf.Lerp(maxScale, 1f, e / half); yield return null; }
            t.localScale = Vector3.one;
        }

        private IEnumerator ShakeCell()
        {
            Vector3 origin = transform.localPosition;
            float duration = 0.3f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float x = Mathf.Sin(elapsed * 60f) * 5f * (1f - elapsed / duration);
                transform.localPosition = origin + new Vector3(x, 0, 0);
                yield return null;
            }
            transform.localPosition = origin;
        }
    }
}