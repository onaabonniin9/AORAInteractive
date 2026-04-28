using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager - Controlador principal del microjuego de memoria
/// Gestiona el flujo completo: rondas, puntuación, temporizador y dificultad
/// </summary>
public class G1_GameManager : MonoBehaviour{
    public static G1_GameManager Instance { get; private set; }

    [Header("Configuración del juego")]
    [SerializeField] private int targetScore = 20;
    [SerializeField] private int gameDuration = 60;
    [SerializeField] private float memorizeTimeBase = 2.5f;
    [SerializeField] private float memorizeTimeMin = 1.0f;
    [SerializeField] private int coinsStartCount = 3;
    [SerializeField] private int coinsMaxCount = 6;

    [Header("Referencias")]
    [SerializeField] private GridController gridController;
    [SerializeField] private G1_UIManager uiManager;

    // Estado del juego
    public enum GameState { Idle, Memorizing, Selecting, RoundEnd, Win, Lose }
    public GameState CurrentState { get; private set; } = GameState.Idle;

    private int score = 0;
    private int timeLeft;
    private int roundNumber = 0;
    private Coroutine timerCoroutine;

    // Propiedades públicas para la UI
    public int Score => score;
    public int TimeLeft => timeLeft;
    public int RoundNumber => roundNumber;
    public int TargetScore => targetScore;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        uiManager.ShowStartScreen();
    }

    public void StartGame()
    {
        score = 0;
        timeLeft = gameDuration;
        roundNumber = 0;

        uiManager.UpdateScore(score);
        uiManager.UpdateTimer(timeLeft);
        uiManager.UpdateRound(roundNumber);
        uiManager.HideAllScreens();

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(TimerCountdown());

        StartNextRound();
    }

    private void StartNextRound()
    {
        roundNumber++;
        uiManager.UpdateRound(roundNumber);

        int coinsCount = CalculateCoinsForRound();
        float memorizeTime = CalculateMemorizeTime();

        CurrentState = GameState.Memorizing;
        gridController.StartRound(coinsCount, memorizeTime);
    }

    private int CalculateCoinsForRound()
    {
        // Cada 2 rondas después de la 3, se añade una moneda más
        int extra = roundNumber > 3 ? (roundNumber - 3) / 2 : 0;
        return Mathf.Min(coinsStartCount + extra, coinsMaxCount);
    }

    private float CalculateMemorizeTime()
    {
        // Reduce 0.2s cada ronda después de la 4
        float reduction = roundNumber > 4 ? (roundNumber - 4) * 0.2f : 0f;
        return Mathf.Max(memorizeTimeMin, memorizeTimeBase - reduction);
    }

    public void OnCoinCorrect()
    {
        score++;
        uiManager.UpdateScore(score);
        uiManager.UpdateProgress((float)score / targetScore);

        // Haptic feedback en iOS
        HapticFeedback.Light();

        if (score >= targetScore)
        {
            WinGame();
        }
    }

    public void OnCoinWrong()
    {
        HapticFeedback.Heavy();
    }

    public void OnRoundComplete()
    {
        if (CurrentState == GameState.Win || CurrentState == GameState.Lose) return;
        CurrentState = GameState.RoundEnd;
        StartCoroutine(DelayNextRound(0.7f));
    }

    private IEnumerator DelayNextRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CurrentState != GameState.Win && CurrentState != GameState.Lose)
            StartNextRound();
    }

    private IEnumerator TimerCountdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            uiManager.UpdateTimer(timeLeft);

            if (timeLeft <= 10)
                uiManager.FlashTimer(); // Aviso visual cuando queda poco tiempo
        }
        LoseGame();
    }

    private void WinGame()
    {
        CurrentState = GameState.Win;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        uiManager.ShowWinScreen(score, gameDuration - timeLeft, roundNumber);
        HapticFeedback.Success();
    }

    // Método público para que otros scripts puedan cambiar el estado
    public void CurrentState_Set(GameState state)
    {
        CurrentState = state;
    }

    private void LoseGame()
    {
        CurrentState = GameState.Lose;
        gridController.DisableInput();
        uiManager.ShowLoseScreen(score, targetScore, roundNumber);
        HapticFeedback.Failure();
    }
}
