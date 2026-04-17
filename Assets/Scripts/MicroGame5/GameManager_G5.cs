using UnityEngine;
using TMPro;

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

    // Control de posiciones ocupadas
    private bool[] occupiedPositions;
    private int totalPositions = 12;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        currentTime = gameTime;

        occupiedPositions = new bool[totalPositions];

        // Spawn inicial
        for (int i = 0; i < 8; i++)
        {
            SpawnEnemy();
        }
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

            if (timerText != null)
                timerText.text = "Tiempo: " + seconds;
        }
    }

    public void AddScoreG5(int points)
    {
        if (currentTime <= 0) return;

        score += points;
        UpdateScoreUI();

        // Cada vez que matas uno → aparece otro
        SpawnEnemy();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab no asignado!");
            return;
        }

        float radius = 5f;

        int randomIndex = -1;

        for (int i = 0; i < 20; i++)
        {
            int index = Random.Range(0, totalPositions);
            if (!occupiedPositions[index])
            {
                randomIndex = index;
                break;
            }
        }

        if (randomIndex == -1)
        {
            randomIndex = Random.Range(0, totalPositions);
        }

        occupiedPositions[randomIndex] = true;

        float angle = (360f / totalPositions) * randomIndex;

        float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

        Vector3 spawnPosition = new Vector3(x, 1f, z);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Mirar a la cámara
        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - enemy.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                enemy.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // 🔥 CORRECCIÓN CLAVE AQUÍ
        Enemy enemyScript = enemy.GetComponentInChildren<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.positionIndex = randomIndex;
        }
        else
        {
            Debug.LogWarning("Enemy script NO encontrado en el prefab");
        }
    }

    public void FreePositionG5(int index)
    {
        if (index >= 0 && index < occupiedPositions.Length)
        {
            occupiedPositions[index] = false;
        }
    }

    void EndGame()
    {
        Debug.Log("FIN DEL JUEGO");

        if (gameOverText != null)
        {
            if (score >= 10)
            {
                gameOverText.text = "¡HAS GANADO!\nPuntuación: " + score;
            }
            else
            {
                gameOverText.text = "HAS PERDIDO\nPuntuación: " + score;
            }

            gameOverText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}