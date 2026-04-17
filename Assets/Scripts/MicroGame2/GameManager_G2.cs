using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager_G2 : MonoBehaviour
{
    public static GameManager_G2 instance;

    [Header("Game State")]
    public int coinsG2 = 0;
    public int coinsToWinG2 = 10;

    public float timeLeftG2 = 90f;
    private bool gameEndedG2 = false;

    [Header("UI In-Game")]
    public TextMeshProUGUI timerTextG2;
    public TextMeshProUGUI coinsTextG2;

    [Header("End Screen")]
    public GameObject endScreenPanelG2;
    public TextMeshProUGUI resultTextG2;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameEndedG2) return;

        HandleTimerG2();
        UpdateUIG2();
    }

    void UpdateUIG2()
    {
        if (timerTextG2 != null)
            timerTextG2.text = "Tiempo: " + Mathf.Ceil(timeLeftG2);

        if (coinsTextG2 != null)
            coinsTextG2.text = "Monedas: " + coinsG2;
    }

    public void AddCoinG2()
    {
        if (gameEndedG2) return;

        coinsG2++;
    }

    void HandleTimerG2()
    {
        timeLeftG2 -= Time.deltaTime;

        if (timeLeftG2 <= 0)
        {
            timeLeftG2 = 0;

            if (coinsG2 >= coinsToWinG2)
                WinGameG2();
            else
                LoseGameG2();
        }
    }

    void WinGameG2()
    {
        EndGameG2(true);
    }

    void LoseGameG2()
    {
        EndGameG2(false);
    }

    void EndGameG2(bool win)
    {
        gameEndedG2 = true;

        if (endScreenPanelG2 != null)
            endScreenPanelG2.SetActive(true);

        if (resultTextG2 != null)
            resultTextG2.text = win ? "¡VICTORIA!" : "GAME OVER";

        Time.timeScale = 0f;
    }

    public void RestartGameG2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}