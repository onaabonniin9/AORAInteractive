using UnityEngine;
using UnityEngine.UI;

public class GazeShoot : MonoBehaviour
{
    public float gazeTimeRequired = 1.0f;

    // 🎯 Crosshair
    public Image crosshair;
    public Color normalColor = Color.white;
    public Color targetColor = Color.green;

    private float gazeTimer = 0f;
    private GameObject currentTarget = null;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 🟢 Apuntando a enemigo
                crosshair.color = targetColor;
                crosshair.transform.localScale = Vector3.one * 1.5f;

                if (currentTarget == hit.collider.gameObject)
                {
                    gazeTimer += Time.deltaTime;

                    if (gazeTimer >= gazeTimeRequired)
                    {
                        GameManager_G5.instance.AddScore(1);
                        Destroy(currentTarget);
                        ResetGaze();
                    }
                }
                else
                {
                    currentTarget = hit.collider.gameObject;
                    gazeTimer = 0f;
                }
            }
            else
            {
                ResetVisual();
                ResetGaze();
            }
        }
        else
        {
            ResetVisual();
            ResetGaze();
        }
    }

    void ResetGaze()
    {
        gazeTimer = 0f;
        currentTarget = null;
    }

    void ResetVisual()
    {
        // ⚪ Estado normal
        crosshair.color = normalColor;
        crosshair.transform.localScale = Vector3.one;
    }
}