using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UIManager - Controla todos los elementos de UI del juego
/// HUD, pantallas de inicio/fin, animaciones de texto
/// </summary>
public class G1_UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Pantalla de inicio")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private Button startButton;

    [Header("Pantalla de victoria")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winScoreText;
    [SerializeField] private TextMeshProUGUI winTimeText;
    [SerializeField] private TextMeshProUGUI winRoundText;
    [SerializeField] private Button winRetryButton;

    [Header("Pantalla de derrota")]
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI loseScoreText;
    [SerializeField] private TextMeshProUGUI loseTargetText;
    [SerializeField] private Button loseRetryButton;

    [Header("Colores timer")]
    [SerializeField] private Color timerNormal = new Color(0f, 1f, 0.8f);
    [SerializeField] private Color timerDanger = new Color(1f, 0.27f, 0.27f);

    private Coroutine timerFlashCoroutine;

    void Start()
    {
        startButton.onClick.AddListener(() => G1_GameManager.Instance.StartGame());
        winRetryButton.onClick.AddListener(() => G1_GameManager.Instance.StartGame());
        loseRetryButton.onClick.AddListener(() => G1_GameManager.Instance.StartGame());
    }

    public void ShowStartScreen()
    {
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void HideAllScreens()
    {
        startScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void ShowWinScreen(int score, int timeUsed, int round)
    {
        winScreen.SetActive(true);
        winScoreText.text = $"{score}€ recuperados";
        winTimeText.text = $"Tiempo: {timeUsed}s";
        winRoundText.text = $"Ronda {round}";
        StartCoroutine(AnimateScreenIn(winScreen));
    }

    public void ShowLoseScreen(int score, int target, int round)
    {
        loseScreen.SetActive(true);
        loseScoreText.text = $"{score}€ de {target}€";
        loseTargetText.text = $"Ronda {round}";
        StartCoroutine(AnimateScreenIn(loseScreen));
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        StartCoroutine(PunchText(scoreText.transform));
    }

    public void UpdateTimer(int time)
    {
        timerText.text = time.ToString();
        timerText.color = time <= 10 ? timerDanger : timerNormal;
    }

    public void UpdateRound(int round)
    {
        roundText.text = round.ToString();
    }

    public void UpdateProgress(float normalized)
    {
        // Animar la barra de progreso
        StopCoroutine("AnimateProgress");
        StartCoroutine(AnimateProgress(progressBar.fillAmount, Mathf.Clamp01(normalized)));
    }

    public void SetPhaseText(string phase)
    {
        phaseText.text = phase;
    }

    public void FlashTimer()
    {
        if (timerFlashCoroutine != null) StopCoroutine(timerFlashCoroutine);
        timerFlashCoroutine = StartCoroutine(FlashTimerCoroutine());
    }

    // -------- Animaciones --------

    private IEnumerator AnimateProgress(float from, float to)
    {
        float elapsed = 0f, duration = 0.3f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            progressBar.fillAmount = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        progressBar.fillAmount = to;
    }

    private IEnumerator PunchText(Transform t)
    {
        float duration = 0.2f;
        float half = duration / 2f;
        float e = 0f;
        while (e < half) { e += Time.deltaTime; t.localScale = Vector3.one * Mathf.Lerp(1f, 1.25f, e / half); yield return null; }
        e = 0f;
        while (e < half) { e += Time.deltaTime; t.localScale = Vector3.one * Mathf.Lerp(1.25f, 1f, e / half); yield return null; }
        t.localScale = Vector3.one;
    }

    private IEnumerator FlashTimerCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            timerText.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            timerText.color = timerDanger;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AnimateScreenIn(GameObject screen)
    {
        CanvasGroup cg = screen.GetComponent<CanvasGroup>();
        if (cg == null) cg = screen.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        float elapsed = 0f, duration = 0.4f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
        cg.alpha = 1f;
    }
}
