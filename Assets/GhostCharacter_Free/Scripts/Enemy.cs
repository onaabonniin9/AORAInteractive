using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 🆕 Guardar la posición que ocupa
    public int positionIndex;

    void OnMouseDown()
    {
        GameManager.instance.AddScore(1);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // 🆕 Liberar la posición cuando muere
        if (GameManager.instance != null)
        {
            GameManager.instance.FreePosition(positionIndex);
        }
    }
}