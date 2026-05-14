using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip coinSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager_G2.instance.AddCoinG2();

            AudioSource.PlayClipAtPoint(
                coinSound,
                transform.position
            );

            Destroy(gameObject);
        }
    }
}