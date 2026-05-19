using UnityEngine;
using UnityEngine.UI;

public class GazeShoot : MonoBehaviour
{
    public float gazeTimeRequired = 1.0f;

    // Crosshair
    public Image crosshair;
    public Color normalColor = Color.white;
    public Color targetColor = Color.green;

    // Sonido de disparo
    public AudioClip shootClip;
    public float shootVolume = 0.5f;

    private float gazeTimer = 0f;
    private GameObject currentTarget = null;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (crosshair != null)
                {
                    crosshair.color = targetColor;
                    crosshair.transform.localScale = Vector3.one * 1.5f;
                }

                if (currentTarget == hit.collider.gameObject)
                {
                    gazeTimer += Time.deltaTime;

                    if (gazeTimer >= gazeTimeRequired)
                    {
                        PlayShootSound();

                        Enemy enemy = currentTarget.GetComponent<Enemy>();

                        if (enemy != null)
                        {
                            enemy.Die();
                        }

                        ResetVisual();
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

    void PlayShootSound()
    {
        if (shootClip != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(shootClip, Camera.main.transform.position, shootVolume);
        }
    }

    void ResetGaze()
    {
        gazeTimer = 0f;
        currentTarget = null;
    }

    void ResetVisual()
    {
        if (crosshair != null)
        {
            crosshair.color = normalColor;
            crosshair.transform.localScale = Vector3.one;
        }
    }
}