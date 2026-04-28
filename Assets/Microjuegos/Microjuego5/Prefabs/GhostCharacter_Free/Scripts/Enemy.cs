using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 🆕 Guardar la posición que ocupa
    public int positionIndex;

    void OnMouseDown()
    {
        GameManager_G5.instance.AddScoreG5(1);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // 🆕 Liberar la posición cuando muere
        if (GameManager_G5.instance != null)
        {
            GameManager_G5.instance.FreePositionG5(positionIndex);
        }
    }
}