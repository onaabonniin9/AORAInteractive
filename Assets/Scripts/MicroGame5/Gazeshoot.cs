using UnityEngine;
using UnityEngine.UI;

public class GazeShoot : MonoBehaviour
{
    public float gazeTimeRequired = 1.0f;

    public Image crosshair;
    public Color normalColor = Color.white;
    public Color targetColor = Color.green;

    private float gazeTimer = 0f;
    private GameObject currentTarget = null;

    private bool hasKilled = false;

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
                crosshair.color = targetColor;
                crosshair.transform.localScale = Vector3.one * 1.5f;

                GameObject hitObject = hit.collider.gameObject;

                if (currentTarget == hitObject)
                {
                    gazeTimer += Time.deltaTime;

                    if (gazeTimer >= gazeTimeRequired && !hasKilled)
                    {
                        hasKilled = true;

                        // 🔥 SOLO LLAMAMOS A DIE()
                        Enemy enemyScript = hit.collider.GetComponentInParent<Enemy>();

                        if (enemyScript != null)
                        {
                            enemyScript.Die();
                        }

                        ResetGaze();
                    }
                }
                else
                {
                    currentTarget = hitObject;
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
        hasKilled = false;
    }

    void ResetVisual()
    {
        crosshair.color = normalColor;
        crosshair.transform.localScale = Vector3.one;
    }
}