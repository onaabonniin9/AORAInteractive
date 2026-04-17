using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARGameMaster : MonoBehaviour
{
    [Header("Referencias")]
    public ARPlayerHead playerHead;
    public Transform xrOrigin;                    // Arrastra el XR Origin aquí
    public GameObject obstaclePrefab;
    public GameObject collectiblePrefab;
    [Header("Debug / Feedback")]
    public GameObject borderIndicatorPrefab;
    public AudioSource collisionAudio;
    public AudioSource collectibleAudio;

    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI timerText;

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
    private void Start()
    {
        StartGame(); // A saco sin UI
    }

    private void Update()
    {
        if (!gameRunning) return;

        // Timer
        timeLeft -= Time.deltaTime;
        //timerText.text = Mathf.Ceil(timeLeft).ToString("00");

        // Daño continuo
        if (hazardCollisionCounter > 0)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 0.4f)
            {
                currentHealth -= 10; 
                damageTimer = 0f;

                if (currentHealth <= 0)
                {
                    //GameOver();
                }
            }
        }

        // Fin del nivel
        if (timeLeft <= 0f)
        {
            //WinLevel();
        }
    }

    // ====================== LLAMADAS DESDE PLAYERHEAD ======================

    public void AddHazardCollision()
    {
        hazardCollisionCounter++;
        if (collisionAudio != null)
        {
            collisionAudio.Play();
        }
    }

    public void RemoveHazardCollision()
    {
        hazardCollisionCounter--;
        if (hazardCollisionCounter <= 0) // Por si se le va la castaña que es muy probable
        {
            hazardCollisionCounter = 0;
            damageTimer = 0f;
        }
    }

    public void CollectibleAdd(GameObject coin)
    {
        // Aquí puedes sumar puntos
        Debug.Log("¡Moneda recogida!");
        Destroy(coin);
        if (collectibleAudio != null)
        {
            collectibleAudio.Play();
        }
    }

    // ====================== CONTROL DEL JUEGO ======================

    public void StartGame()
    {
        // Calibración del túnel
        if (xrOrigin != null)
        {
            xrOrigin.position = Camera.main.transform.position;
            xrOrigin.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }

        timeLeft = levelDuration;
        currentHealth = maxHealth;
        gameRunning = true;

        SpawnBorders();

        // Activar spawner después de 3 segundos de calibración
        Invoke("ActivateSpawner", 3f);
    }

    private void ActivateSpawner()
    {
        // Aquí activaremos el spawneo (lo haremos con InvokeRepeating para que sea más fácil de controlar)
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

        if (Random.value < 0.78f) // Obstáculo
        {
            Quaternion rot = Quaternion.LookRotation(flatForward) * Quaternion.Euler(0, -90f, 0);
            GameObject obs = Instantiate(obstaclePrefab, spawnPos, rot);
            obs.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            obs.GetComponent<ObstacleMovement>().origin = xrOrigin;
        }
        else // Collectible
        {
            Quaternion rot = Quaternion.LookRotation(flatForward) * Quaternion.Euler(0, -90f, 0);
            GameObject obs = Instantiate(collectiblePrefab, spawnPos, rot);
            obs.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            obs.GetComponent<ObstacleMovement>().origin = xrOrigin;
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

        // Instanciamos TEMPORAL para medir tamaño real
        GameObject temp = Instantiate(borderIndicatorPrefab, Vector3.zero, Quaternion.identity);
        Renderer rend = temp.GetComponentInChildren<Renderer>();

        float halfWidth = 0.5f; // fallback

        if (rend != null)
        {
            halfWidth = rend.bounds.size.x / 2f;
        }

        Destroy(temp);

        // Offset total = rango jugable + mitad del prefab
        float totalOffset = lateralRange + halfWidth;

        // LEFT
        Vector3 leftPos = xrOrigin.position 
            + flatForward * spawnDistance
            - flatRight * totalOffset;

        Instantiate(borderIndicatorPrefab, leftPos, rot);

        // RIGHT
        Vector3 rightPos = xrOrigin.position 
            + flatForward * spawnDistance
            + flatRight * totalOffset;

        Instantiate(borderIndicatorPrefab, rightPos, rot);
    }

    private void GameOver()
    {
        gameRunning = false;
        CancelInvoke();
        Debug.Log("GAME OVER");
        // Aquí mostrarías UI de derrota
    }

    private void WinLevel()
    {
        gameRunning = false;
        CancelInvoke();
        Debug.Log("¡NIVEL SUPERADO!");
        // Aquí mostrarías UI de victoria
    }
}