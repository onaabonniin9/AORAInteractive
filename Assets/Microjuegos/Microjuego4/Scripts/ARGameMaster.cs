using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using UnityEngine.SceneManagement;

public class ARGameMaster : MonoBehaviour
{
    [Header("Referencias")]
    public ARPlayerHead playerHead;
    public Transform xrOrigin;
    public GameObject obstaclePrefab;
    public GameObject collectiblePrefab;
    public GameObject bonusPrefab;

    [Header("Debug / Feedback")]
    public GameObject borderIndicatorPrefab;
    public AudioSource collisionAudio;
    public AudioSource collectibleAudio;
    public AudioSource bonusAudio;
    public AudioSource gameAudio;
    public AudioSource menuAudio;

    [Header("UI")]
    public Slider sliderBarraVida;
    public TextMeshProUGUI textValueBarraVida;
    public TextMeshProUGUI textValueCoins;
    public TextMeshProUGUI textValueTiempo;
    public TextMeshProUGUI textValueVelocidadNivel;
    public TextMeshProUGUI textValueDesplazamientoLateral;
    public TextMeshProUGUI textNotificacion;
    public TextMeshProUGUI textPuntosFinal;
    public TextMeshProUGUI textPuntosFinalDerrota;
    public TextMeshProUGUI textMotivoDerrota;
    [SerializeField] public GameObject panelMaingame;
    [SerializeField] public GameObject panelVictoria;
    [SerializeField] public GameObject panelDerrota;
    private float notificacionAlpha = 0f;
    private float notificacionCounter = 0;

    [Header("Ajustes")]
    public float levelDuration = 60f;
    public float obstacleSpeed = 10f;
    public float spawnInterval = 1.0f;
    public float spawnDistance = 22f;
    public float lateralRange = 4.5f;

    private float timeLeft;
    private bool gameRunning = false;
    private float lateralMul = 3f;

    private int hazardCollisionCounter = 0;
    private float damageTimer = 0f;

    private float oldCurrentHealth = 100f;
    private float currentHealth = 100f;
    private float maxHealth = 100f;
    private int recoveryCooldown = 0;

    private int oldCoins = 0;
    private int coins = 0;
    public int coinsToWin = 10;

    void Start()
    {
        //
        menuAudio.Play();
    }

    public void EmpezarJuego()
    {
        StartGame();
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        obstacleSpeed = nuevaVelocidad;
        textValueVelocidadNivel.text = "" + obstacleSpeed;
    }

    public void CambiarDesplazamiento(float nuevoDesplazamiento)
    {
        lateralMul = nuevoDesplazamiento;
        textValueDesplazamientoLateral.text = "" + lateralMul;
    }

    private void Victoria()
    {
        textPuntosFinal.text = "x " + coins;
        panelVictoria.SetActive(true);
        panelMaingame.SetActive(false);
        menuAudio.Play();
        gameAudio.Stop();
        currentHealth = 100f;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARHazard")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARCollectible")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARBonus")) Destroy(go);
        timeLeft = levelDuration;
        gameRunning = false;
        CancelInvoke();
        WinLevel();
    }

    public void SalirPartida()
    {
        menuAudio.Play();
        gameAudio.Stop();
        currentHealth = 100f;
        coins = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARHazard")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARCollectible")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARBonus")) Destroy(go);
        timeLeft = levelDuration;
        gameRunning = false;
        CancelInvoke();
        UpdatearNotificacion("FIN");
    }
    void UpdatearNotificacion(string texto)
    {
        notificacionAlpha = 1f;
        notificacionCounter = 100;
        textNotificacion.text = texto;

        Color color = textNotificacion.color;
        color.a = notificacionAlpha;
        textNotificacion.color = color;

        //textNotificacion.transform.localScale = new UnityEngine.Vector3(1.15f, 1.15f, 1.15f);
    }

    void Update()
    {
        if (!gameRunning) return;

        timeLeft -= Time.deltaTime;

        if (textValueTiempo != null)
            textValueTiempo.text = "" + Mathf.Ceil(timeLeft);

        if (currentHealth <= 0)
        {
            GameOver("TE QUEDASTES SIN VIDA");
        }

        if (currentHealth < 100)
        {
            if (recoveryCooldown > 30)
            {
                currentHealth += 0.05f;
            }
        }

        if (oldCurrentHealth != currentHealth)
        {
            oldCurrentHealth = currentHealth;
            sliderBarraVida.value = Mathf.Clamp(currentHealth, 0f, 100f);
        
             if (textValueBarraVida != null)
            textValueBarraVida.text = $"{Mathf.Round(currentHealth)}%";
        }

        if (oldCoins != coins)
        {
            oldCoins = coins;
            textValueCoins.text = $"x {coins}";
        }

        if (currentHealth > 100f)
        {
            currentHealth = 100f;
        }

        if (recoveryCooldown < 40)
        {
            recoveryCooldown++;    
        }
        
        /*
        // Esto era innecesariamente complicado y poco factible para un microjuego. F en el chat.
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
        */

        if (timeLeft <= 0f)
        {
            if (coins >= 10)
            {
                Victoria();
            }
            else
            {
                GameOver("NO CONSEGUISTES SUFICIENTES MONEDAS");
            }
        }

        if (notificacionCounter > 0)
        {
            notificacionCounter--;
            if (notificacionCounter <= 50)
            {
                Color color = textNotificacion.color;
                color.a -= 0.02f;
                textNotificacion.color = color;
            }
        }
        else
        {
            notificacionCounter = 0;
            Color color = textNotificacion.color;
            if (textNotificacion.color.a != 0f)
            {
                color.a = 0f;
                textNotificacion.color = color;
            }
        }
    }

    public void AddHazardCollision()
    {
        hazardCollisionCounter++;

        if (collisionAudio != null)
            collisionAudio.Play();
    }

    public void GetHazardCollision(GameObject hazard)
    {
        Destroy(hazard);

        currentHealth -= 20f;
        recoveryCooldown = 0;

        if (collisionAudio != null)
            collisionAudio.Play();

        UpdatearNotificacion("CHOCASTE");
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

        UpdatearNotificacion("+1 PUNTO");
    }

    public void GetBonus(GameObject coin)
    {
        Destroy(coin);

        if (bonusAudio != null)
            bonusAudio.Play();

        coins++;

        ClearAllHazards();

        UpdatearNotificacion("BONUS");
    }

    private void ClearAllHazards()
    {
        foreach (GameObject hazard in GameObject.FindGameObjectsWithTag("ARHazard"))
        {
            Destroy(hazard);
        }
    }

    public void StartGame()
    {
        if (xrOrigin != null)
        {
            xrOrigin.position = Camera.main.transform.position;
            xrOrigin.rotation = UnityEngine.Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }

        UpdatearNotificacion("START!");

        timeLeft = levelDuration;
        currentHealth = maxHealth;
        coins = 0;
        gameRunning = true;
        menuAudio.Stop();
        gameAudio.Play();

        //SpawnBorders();

        Invoke("ActivateSpawner", 3f);
    }

    private void ActivateSpawner()
    {
        InvokeRepeating("SpawnElement", 0f, spawnInterval);
    }

    private void SpawnElement()
    {
        if (!gameRunning) return;

        UnityEngine.Vector3 flatForward = xrOrigin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        UnityEngine.Vector3 flatRight = xrOrigin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        UnityEngine.Vector3 spawnPos = xrOrigin.position
            + flatForward * spawnDistance
            + flatRight * Random.Range(-lateralRange, lateralRange);

        float randomVal = Random.value;
        if (randomVal < 0.78f && randomVal > 0.1f)
        {
            // OBstaculo
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(flatForward) * UnityEngine.Quaternion.Euler(0, -90f, 0);
            GameObject obs = Instantiate(obstaclePrefab, spawnPos, rot);
            // No es factible...
            /*
            float randomScale = Random.Range(3f, 60f);
            obs.transform.localScale = new UnityEngine.Vector3(
                randomScale,
                obs.transform.localScale.y,
                obs.transform.localScale.z
            );
            */
            obs.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            obs.GetComponent<ObstacleMovement>().origin = xrOrigin;
            obs.GetComponent<ObstacleMovement>().lateralMultiplier = lateralMul;

            float auxiliarRandom = Random.value;
            if (auxiliarRandom <= 0.33f)
            {
                // Esque si no a veces pasan los 60 segundos y no han spawneado 10 moneditas porque el RNG es asi...
                spawnPos = xrOrigin.position
                + flatForward * spawnDistance
                + flatRight * Random.Range(-lateralRange, lateralRange);
                rot = UnityEngine.Quaternion.LookRotation(flatForward) * UnityEngine.Quaternion.Euler(-90f, 0f, 0);
                GameObject coin = Instantiate(collectiblePrefab, spawnPos, rot);
                coin.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
                coin.GetComponent<ObstacleMovement>().origin = xrOrigin;
                coin.GetComponent<ObstacleMovement>().lateralMultiplier = lateralMul;
            }
        }
        else if (randomVal <= 0.1f)
        {
            // Bonus
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(flatForward) * UnityEngine.Quaternion.Euler(-90f, 0f, 0);
            GameObject coin = Instantiate(bonusPrefab, spawnPos, rot);
            coin.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            coin.GetComponent<ObstacleMovement>().origin = xrOrigin;
            coin.GetComponent<ObstacleMovement>().lateralMultiplier = lateralMul;
        }
        else
        {
            // Puntos Normales
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(flatForward) * UnityEngine.Quaternion.Euler(-90f, 0f, 0);
            GameObject coin = Instantiate(collectiblePrefab, spawnPos, rot);
            coin.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
            coin.GetComponent<ObstacleMovement>().origin = xrOrigin;
            coin.GetComponent<ObstacleMovement>().lateralMultiplier = lateralMul;
        }
    }

    private void SpawnBorders()
    {
        if (borderIndicatorPrefab == null || xrOrigin == null) return;

        UnityEngine.Vector3 flatForward = xrOrigin.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        UnityEngine.Vector3 flatRight = xrOrigin.right;
        flatRight.y = 0;
        flatRight.Normalize();

        UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(flatForward) * UnityEngine.Quaternion.Euler(0, -90f, 0);

        GameObject temp = Instantiate(borderIndicatorPrefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
        Renderer rend = temp.GetComponentInChildren<Renderer>();

        float halfWidth = rend != null ? rend.bounds.size.x / 2f : 0.5f;

        Destroy(temp);

        float totalOffset = lateralRange + halfWidth;

        UnityEngine.Vector3 leftPos = xrOrigin.position + flatForward * spawnDistance - flatRight * totalOffset;
        UnityEngine.Vector3 rightPos = xrOrigin.position + flatForward * spawnDistance + flatRight * totalOffset;

        Instantiate(borderIndicatorPrefab, leftPos, rot);
        Instantiate(borderIndicatorPrefab, rightPos, rot);
    }

    private void GameOver(string motivo)
    {
        textPuntosFinalDerrota.text = "x " + coins;
        panelDerrota.SetActive(true);
        panelMaingame.SetActive(false);
        menuAudio.Play();
        gameAudio.Stop();
        currentHealth = 100f;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARHazard")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARCollectible")) Destroy(go);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ARBonus")) Destroy(go);
        timeLeft = levelDuration;
        gameRunning = false;
        CancelInvoke();
        textMotivoDerrota.text = motivo;
    }

    private void WinLevel()
    {
        gameRunning = false;
        CancelInvoke();

        Debug.Log("¡HAS GANADO!");

        GlobalGameManager.instance.WinLevel(coins);
    }

    public void RecargarEscena()
    {
        // Por si cosas raras al final pongo esto, es un poco cutre pero meh...
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}