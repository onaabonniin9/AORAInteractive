using UnityEngine;

public class GazeShoot : MonoBehaviour
{
    public float gazeTimeRequired = 1.0f;
    public GameObject enemyPrefab;

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
                if (currentTarget == hit.collider.gameObject)
                {
                    gazeTimer += Time.deltaTime;

                    if (gazeTimer >= gazeTimeRequired)
                    {
                        Vector3 position = hit.collider.transform.position;

                        GameManager.instance.AddScore(1);
                        Destroy(currentTarget);
                        ResetGaze();

                        Invoke(nameof(SpawnEnemy), 1.5f);
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
                ResetGaze();
            }
        }
        else
        {
            ResetGaze();
        }
    }

    void SpawnEnemy()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(-5f, 5f),
            1f,
            Random.Range(2f, 6f)
        );

        Instantiate(enemyPrefab, newPosition, Quaternion.identity);
    }

    void ResetGaze()
    {
        gazeTimer = 0f;
        currentTarget = null;
    }
}