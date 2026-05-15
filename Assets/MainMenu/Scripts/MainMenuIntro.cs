using UnityEngine;
using System.Collections;

public class MainMenuIntro : MonoBehaviour
{
    public GameObject splashImage;
    public GameObject mainMenuImage;

    public float splashDuration = 3f;

    IEnumerator Start()
    {
        splashImage.SetActive(true);
        mainMenuImage.SetActive(false);

        yield return new WaitForSeconds(splashDuration);

        splashImage.SetActive(false);
        mainMenuImage.SetActive(true);
    }
}
