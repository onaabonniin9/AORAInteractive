using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip hurtSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    void OnDestroy()
    {
        audioSource = null;
    }
}