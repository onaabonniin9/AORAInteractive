using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        audioSource.Play();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        float volume = Mathf.Clamp01(1f - distance / 15f);

        audioSource.volume = volume;
    }
}
