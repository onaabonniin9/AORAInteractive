using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;
    public bool lastLevelWon;
    public int currentLevel = 1;
    public int totalLevels = 5;
    public GameObject finalScreenPanel;
    public TextMeshProUGUI finalText;
    public TextMeshProUGUI finalScoreText;
    public int totalScore = 0;

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
        LoadLevel(currentLevel);
    }

    public void WinLevel(int levelScore)
    {
        totalScore += levelScore;
        lastLevelWon = true;

        if (currentLevel >= totalLevels)
        {
            ShowFinalScreen();
        }
        else
        {
            currentLevel++;
            SceneManager.LoadScene("InterLevel");
        }
    }

    public void LoseLevel()
    {
        lastLevelWon = false;

        if (currentLevel == 1)
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("InterLevel");
    }

    public void ContinueGame()
    {
        LoadLevel(currentLevel);
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Microgame" + level);
    }

    void ShowFinalScreen()
    {
        if (finalScreenPanel != null){
            finalScreenPanel.SetActive(true);
        }
        if (finalText != null){
            finalText.text =
                "HAS COMPLETADO TODOS LOS MICROJUEGOS";
        }
        if (finalScoreText != null){
            finalScoreText.text =
                "PUNTUACIÓN TOTAL: " + totalScore;

        Time.timeScale = 0f;
        }
    }
}
