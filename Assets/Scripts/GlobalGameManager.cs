using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    [Header("Progress")]
    public int currentGame = 1;
    public int totalMoney = 0;
    public int maxGames = 5;

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

    public void AddMoney(int coins)
    {
        int money = coins * 10000;
        totalMoney += money;
    }

    public void NextGame()
    {
        currentGame++;
    }

    public void LoadNextScene()
    {
        if (currentGame > maxGames)
        {
            SceneManager.LoadScene("FinalScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ResetGame()
    {
        currentGame = 1;
        totalMoney = 0;
        SceneManager.LoadScene("MainMenu");
    }
}