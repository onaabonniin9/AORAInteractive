using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    [Header("Estado de nivel")]
    public bool lastLevelWon;
    public bool retryCurrentLevel;

    [Header("Progresión")]
    public int currentLevel = 1;
    public int totalLevels = 5;

    [Header("Puntuación global")]
    public int totalScore = 0;

    [Header("UI Final")]
    public GameObject finalScreenPanel;
    public TextMeshProUGUI finalText;
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        currentLevel = 1;
        totalScore = 0;
        LoadLevel(currentLevel);
    }

    public void WinLevel(int levelScore)
    {
        totalScore += levelScore;

        lastLevelWon = true;
        retryCurrentLevel = false;

        if (currentLevel >= totalLevels)
        {
            ShowFinalScreen();
        }
        else
        {
            currentLevel++;
            StartCoroutine(LoadInterLevel());
        }
    }

    public void LoseLevel()
    {
        lastLevelWon = false;
        retryCurrentLevel = true;

        StartCoroutine(LoadInterLevel());
    }

    public void ContinueGame()
    {
        LoadLevel(currentLevel);
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Microjuego" + level);
    }

    IEnumerator LoadInterLevel()
    {
        yield return null;
        Time.timeScale = 1f;
        SceneManager.LoadScene("InterLevel");
    }

    void ShowFinalScreen()
    {
        if (finalScreenPanel != null)
            finalScreenPanel.SetActive(true);

        if (finalText != null)
            finalText.text = "HAS COMPLETADO TODOS LOS MICROJUEGOS";

        if (finalScoreText != null)
            finalScoreText.text =
                "PUNTUACIÓN TOTAL: " + totalScore +
                " (€ " + (totalScore * 10000) + ")";

        Time.timeScale = 0f;
    }
}