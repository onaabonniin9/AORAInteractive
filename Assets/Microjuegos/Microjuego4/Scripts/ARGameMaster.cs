using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARGameMaster : MonoBehaviour
{
    [Header("Referencias")]
    public ARPlayerHead playerHead;
    public Transform xrOrigin;
    public GameObject obstaclePrefab;
    public GameObject collectiblePrefab;

    [Header("Debug / Feedback")]
    public GameObject borderIndicatorPrefab;
    public AudioSource collisionAudio;
    public AudioSource collectibleAudio;

    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinsText;

    [Header("Ajustes")]
    public float levelDuration = 60f;
    public float obstacleSpeed = 10f;
    public float spawnInterval = 1.0f;
    public float spawnDistance = 22f;
    public float lateralRange = 4.5f;

    private float timeLeft;
    private bool gameRunning = false;

    private int hazardCollisionCounter = 0;
    private float damageTimer = 0f;

    private float currentHealth = 100f;
    private float maxHealth = 100f;

    private int coins = 0;
    public int coinsToWin = 10;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameRunning) return;

        timeLeft -= Time.deltaTime;

        if (timerText != null)
            timerText.text = "Tiempo: " + Mathf.Ceil(timeLeft);

        
        if (hazardCollisionCounter > 0)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= 0.4f)
            {
                currentHealth -= 10;
                damageTimer = 0f;

                if (currentHealth <= 0)
                {
                    GameOver();
                }
            }
        }

        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    public void AddHazardCollision()
    {
        hazardCollisionCounter++;

        if (collisionAudio != null)
            collisionAudio.Play();
    }

    public void RemoveHazardCollision()
    {
        hazardCollisionCounter--;

        if (hazardCollisionCounter <= 0)
        {
            hazardCollisionCounter = 0;
            damageTimer = 0f;
        }
    }

    public void CollectibleAdd(GameObject coin)
    {
        Destroy(coin);

        coins++;

        if (collectibleAudio != null)
            collectibleAudio.Play();

        if (coinsText != null)
            coinsText.text = "Monedas: " + coins + "/" + coinsToWin;

        if (coins >= coinsToWin)
        {
            WinLevel();
        }
    }

    public void StartGame()
    {
        if (xrOrigin != null)
        {
            xrOrigin.position = Camera.main.transform.position;
            xrOrigin.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }

        timeLeft = levelDuration;
        currentHealth = maxHealth;
        coins = 0;
        gameRunning = true;

        SpawnBorders();

        Invoke("ActivateSpawner", 3f);
    }

    private void ActivateSpawner()
    {
        InvokeRepeating("SpawnElement", 0f, spawnInterval);
    }

    private void SpawnElement()
    {
        if (!gameRunning) return;

        Vector3 flatForward = xrOrigin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        Vector3 flatRight = xrOrigin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        Vector3 spawnPos = xrOrigin.position
            + flatForward * spawnDistance
            + flatRight * Random.Range(-lateralRange, lateralRange);

        Quaternion rot = Quaternion.LookRotation(flatForward) * Quaternion.Euler(0, -90f, 0);

        if (Random.value < 0.78f)
        {
            GameObject obs = Instantiate(obstaclePrefab, spawnPos, rot);
            obs.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            obs.GetComponent<ObstacleMovement>().origin = xrOrigin;
        }
        else
        {
            GameObject coin = Instantiate(collectiblePrefab, spawnPos, rot);
            coin.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            coin.GetComponent<ObstacleMovement>().origin = xrOrigin;
        }
    }

    private void SpawnBorders()
    {
        if (borderIndicatorPrefab == null || xrOrigin == null) return;

        Vector3 flatForward = xrOrigin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        Vector3 flatRight = xrOrigin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        Quaternion rot = Quaternion.LookRotation(flatForward) * Quaternion.Euler(0, -90f, 0);

        GameObject temp = Instantiate(borderIndicatorPrefab, Vector3.zero, Quaternion.identity);
        Renderer rend = temp.GetComponentInChildren<Renderer>();

        float halfWidth = rend != null ? rend.bounds.size.x / 2f : 0.5f;

        Destroy(temp);

        float totalOffset = lateralRange + halfWidth;

        Vector3 leftPos = xrOrigin.position + flatForward * spawnDistance - flatRight * totalOffset;
        Vector3 rightPos = xrOrigin.position + flatForward * spawnDistance + flatRight * totalOffset;

        Instantiate(borderIndicatorPrefab, leftPos, rot);
        Instantiate(borderIndicatorPrefab, rightPos, rot);
    }

    private void GameOver()
    {
        gameRunning = false;
        CancelInvoke();

        Debug.Log("GAME OVER");

        GlobalGameManager.instance.LoseLevel();
    }

    private void WinLevel()
    {
        gameRunning = false;
        CancelInvoke();

        Debug.Log("¡HAS GANADO!");

        GlobalGameManager.instance.WinLevel(coins);
    }
}