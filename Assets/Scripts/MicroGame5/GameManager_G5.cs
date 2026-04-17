using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager_G5 : MonoBehaviour
{
    public static GameManager_G5 instance;

    [Header("Game State")]
    public int coinsG5 = 0;
    public int coinsToWinG5 = 10;

    public float timeLeftG5 = 90f;
    private bool gameEndedG5 = false;

    [Header("UI In-Game")]
    public TextMeshProUGUI timerTextG5;
    public TextMeshProUGUI coinsTextG5;

    [Header("End Screen")]
    public GameObject endScreenPanelG5;
    public TextMeshProUGUI resultTextG5;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameEndedG5) return;

        HandleTimerG5();
        UpdateUIG5();
    }

    void UpdateUIG5()
    {
        if (timerTextG5 != null)
            timerTextG5.text = "Tiempo: " + Mathf.Ceil(timeLeftG5);

        if (coinsTextG5 != null)
            coinsTextG5.text = "Monedas: " + coinsG5;
    }

    public void AddCoinG5()
    {
        if (gameEndedG5) return;

        coinsG5++;
    }

    void HandleTimerG5()
    {
        timeLeftG5 -= Time.deltaTime;

        if (timeLeftG5 <= 0)
        {
            timeLeftG5 = 0;

            if (coinsG5 >= coinsToWinG5)
                WinGameG5();
            else
                LoseGameG5();
        }
    }

    void WinGameG5()
    {
        EndGameG5(true);
    }

    void LoseGameG5()
    {
        EndGameG5(false);
    }

    void EndGameG5(bool win)
    {
        gameEndedG5 = true;

        if (endScreenPanelG5 != null)
            endScreenPanelG5.SetActive(true);

        if (resultTextG5 != null)
            resultTextG5.text = win ? "¡VICTORIA!" : "GAME OVER";

        Time.timeScale = 0f;
    }

    public void RestartGameG5()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}