using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager_G2.instance.AddCoinG2();
            Destroy(gameObject);
        }
    }
}
