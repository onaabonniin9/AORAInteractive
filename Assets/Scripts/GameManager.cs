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

    public TextMeshProUGUI gameOverText;

    // 🆕 Control de posiciones ocupadas
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

        // 🆕 Inicializar array de posiciones
        occupiedPositions = new bool[totalPositions];

        // 🔥 SPAWN INICIAL (8 fantasmas)
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
            timerText.text = "Tiempo: " + seconds;
        }
    }

    public void AddScore(int points)
    {
        if (currentTime <= 0) return;

        score += points;
        UpdateScoreUI();

        // 🔁 Cada vez que matas uno → aparece otro
        SpawnEnemy();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Puntos: " + score;
    }

    void SpawnEnemy()
    {
        float radius = 5f;

        // 🔍 Buscar posición libre
        int randomIndex = -1;

        for (int i = 0; i < 20; i++) // intentos
        {
            int index = Random.Range(0, totalPositions);
            if (!occupiedPositions[index])
            {
                randomIndex = index;
                break;
            }
        }

        // ❗ fallback (por si todas están ocupadas)
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

        // 👻 HACER QUE MIRE A LA CÁMARA
        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - enemy.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                enemy.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // 🆕 Guardar índice en el enemigo
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.positionIndex = randomIndex;
        }
    }

    // 🆕 Liberar posición cuando muere
    public void FreePosition(int index)
    {
        if (index >= 0 && index < occupiedPositions.Length)
        {
            occupiedPositions[index] = false;
        }
    }

    void EndGame()
    {
        Debug.Log("FIN DEL JUEGO");

        if (score >= 10)
        {
            gameOverText.text = "¡HAS GANADO!\nPuntuación: " + score;
        }
        else
        {
            gameOverText.text = "HAS PERDIDO\nPuntuación: " + score;
        }

        gameOverText.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }
}