using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager_G5 : MonoBehaviour
{
    public static GameManager_G5 instance;

    [Header("Game State")]
    public int score = 0;
    public int scoreToWin = 10; 
    public float gameTime = 60f;
    private float currentTime;
    
    private bool gameEnded = false;
    private bool gameStarted = false;

    [Header("UI In-Game")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI textoInicial;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    private List<Vector3> activeEnemyPositions = new List<Vector3>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        Time.timeScale = 1f;
    }

    void Start()
    {
        currentTime = gameTime;

        if (textoInicial != null)
            textoInicial.gameObject.SetActive(true);

        UpdateScoreUI();
        UpdateTimerUI();

        Time.timeScale = 0f; 
    }

    void Update()
    {
        if (!gameStarted)
        {
            bool isTouchingScreen = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || isTouchingScreen)
            {
                StartGame();
            }
            return;
        }

        if (gameEnded) return;

        HandleTimer();
    }

    void HandleTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                UpdateTimerUI();
                EndGame();
            }
            else
            {
                UpdateTimerUI();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;

        if (textoInicial != null)
            textoInicial.gameObject.SetActive(false);

        SpawnEnemy();
    }

    public void AddScoreG5(int points)
    {
        if (!gameStarted || gameEnded || currentTime <= 0) return;

        score += points;
        UpdateScoreUI();

        SpawnEnemy();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Tiempo: " + Mathf.CeilToInt(currentTime);
    }

    void SpawnEnemy()
    {
        if (!gameStarted || gameEnded || currentTime <= 0) return;
        if (enemyPrefab == null) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        activeEnemyPositions.Add(spawnPosition);

        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - enemy.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
                enemy.transform.rotation = Quaternion.LookRotation(direction);
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
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(minRadius, maxRadius);

            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Random.Range(minHeight, maxHeight);

            spawnPosition = new Vector3(x, y, z);
            bool tooClose = false;

            foreach (Vector3 existing in activeEnemyPositions)
            {
                if (Vector3.Distance(spawnPosition, existing) < minimumDistanceBetweenEnemies)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return spawnPosition;
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
                activeEnemyPositions.Add(enemy.transform.position);
        }
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        bool playerWon = score >= scoreToWin;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
            Destroy(e.gameObject);

        Time.timeScale = 0f;

        if (GlobalGameManager.instance != null)
        {
            if (playerWon)
                GlobalGameManager.instance.WinLevel(score);
            else
                GlobalGameManager.instance.LoseLevel();
        }
        else
        {
            Debug.LogWarning("GlobalGameManager no detectado.");
        }
    }
}