using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public GameObject enemyPrefab;

    public TextMeshProUGUI scoreText;

    public float gameTime = 60f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    
    public GameObject gameOverText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        currentTime = gameTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                EndGame();
            }

            int seconds = Mathf.CeilToInt(currentTime);
            timerText.text = "Tiempo: " + seconds;
        }
    }

    public void AddScore(int points)
    {
        if (currentTime <= 0) return;

        score += points;
        UpdateScoreUI();
        SpawnEnemy();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Puntos: " + score;
    }

    void SpawnEnemy()
    {
        float radius = 5f;
        int totalPositions = 12;

        int randomIndex = Random.Range(0, totalPositions);
        float angle = (360f / totalPositions) * randomIndex;

        float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

        Vector3 spawnPosition = new Vector3(x, 0.5f, z);

        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);

        if (colliders.Length == 0)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void EndGame()
    {
        string mensaje;

        if (score >= 10)
        {
            mensaje = "¡HAS GANADO!\nPuntuación: " + score;
        }
        else
        {
            mensaje = "HAS PERDIDO\nPuntuación: " + score;
        }

        gameOverText.GetComponent<TextMeshProUGUI>().text = mensaje;

        gameOverText.SetActive(true);

        Time.timeScale = 0f;
    }
}