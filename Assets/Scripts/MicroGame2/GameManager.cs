using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public int coins = 0;
    public int coinsToWin = 10;

    public float timeLeft = 90f;
    private bool gameEnded = false;

    [Header("UI In-Game")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinsText;

    [Header("End Screen")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI resultText;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameEnded) return;

        HandleTimer();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Tiempo: " + Mathf.Ceil(timeLeft);

        if (coinsText != null)
            coinsText.text = "Monedas: " + coins;
    }

    public void AddCoin()
    {
        if (gameEnded) return;

        coins++;

    }

    void WinGame()
    {
        EndGame(true);
    }

    void LoseGame()
    {
        EndGame(false);
    }

    void HandleTimer()
    {
    timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
        timeLeft = 0;

            if (coins >= coinsToWin)
                WinGame();
            else
                LoseGame();
        }
    }

    void EndGame(bool win)
    {
        gameEnded = true;

        if (endScreenPanel != null)
            endScreenPanel.SetActive(true);

        if (resultText != null)
            resultText.text = win ? "¡VICTORIA!" : "GAME OVER";

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}