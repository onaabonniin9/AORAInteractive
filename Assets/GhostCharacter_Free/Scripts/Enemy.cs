using UnityEngine;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int positionIndex;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return; // 🔒 evita doble muerte
        isDead = true;

        // 🔓 liberar posición
        GameManager_G5.instance.FreePositionG5(positionIndex);

        // ➕ sumar puntos
        GameManager_G5.instance.AddScoreG5(1);

        // 🚫 evitar detección
        gameObject.tag = "Untagged";

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 💀 destruir
        Destroy(gameObject);
    }
}