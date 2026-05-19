using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Guardar la posición que ocupa
    public int positionIndex;

    private bool isDead = false;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Die()
    {
        // Evitar que muera varias veces
        if (isDead) return;

        isDead = true;

        // Sumar punto
        if (GameManager_G5.instance != null)
        {
            GameManager_G5.instance.AddScoreG5(1);
        }

        // Reproducir sonido de moneda
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Ocultar visualmente el fantasma al morir
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        // Destruir enemigo después de que suene la moneda
        Destroy(gameObject, 0.8f);
    }

    void OnMouseDown()
    {
        Die();
    }

    void OnDestroy()
    {
        // Liberar la posición cuando muere
        if (GameManager_G5.instance != null)
        {
            GameManager_G5.instance.FreePositionG5(positionIndex);
        }
    }
}