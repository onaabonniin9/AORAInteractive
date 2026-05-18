using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager_G5 : MonoBehaviour
{
    public static GameManager_G5 instance;

    public int score = 0;
    public GameObject enemyPrefab;

    public TextMeshProUGUI scoreText;

    public float gameTime = 60f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI gameOverText;

    private bool gameEnded = false;

    private List<Vector3> activeEnemyPositions = new List<Vector3>();

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        currentTime = gameTime;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        UpdateScoreUI();
        UpdateTimerUI();

        // 1 fantasma generado + 1 fantasma manual en escena = 2 simultáneos
        for (int i = 0; i < 1; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        if (gameEnded) return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                UpdateTimerUI();
                EndGame();
                return;
            }

            UpdateTimerUI();
        }
    }

    public void AddScoreG5(int points)
    {
        if (gameEnded || currentTime <= 0) return;

        score += points;
        UpdateScoreUI();

        SpawnEnemy();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(currentTime);
            timerText.text = "Tiempo: " + seconds;
        }
    }

    void SpawnEnemy()
    {
        if (gameEnded || currentTime <= 0) return;
        if (enemyPrefab == null) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        activeEnemyPositions.Add(spawnPosition);

        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - enemy.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                enemy.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float minRadius = 3.4f;
        float maxRadius = 4.4f;
        float minHeight = 0.7f;
        float maxHeight = 1.6f;

        float minimumDistanceBetweenEnemies = 1.2f;

        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < 30; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomRadius = Random.Range(minRadius, maxRadius);

            float x = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
            float z = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
            float y = Random.Range(minHeight, maxHeight);

            spawnPosition = new Vector3(x, y, z);

            bool tooClose = false;

            foreach (Vector3 existingPosition in activeEnemyPositions)
            {
                if (Vector3.Distance(spawnPosition, existingPosition) < minimumDistanceBetweenEnemies)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                return spawnPosition;
            }
        }

        return spawnPosition;
    }

    public void FreePositionG5(int index)
    {
        activeEnemyPositions.Clear();

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                activeEnemyPositions.Add(enemy.transform.position);
            }
        }
    }

    void EndGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        bool win = score >= 10;

        if (gameOverText != null)
        {
            gameOverText.text = win
                ? "¡MICROJUEGO SUPERADO!\nPuntuación: " + score
                : "MICROJUEGO NO SUPERADO\nPuntuación: " + score;

            gameOverText.gameObject.SetActive(true);
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        if (GlobalGameManager.instance != null)
        {
            if (win)
            {
                GlobalGameManager.instance.WinLevel(score);
            }
            else
            {
                GlobalGameManager.instance.LoseLevel();
            }
        }

        Time.timeScale = 0f;
    }
}