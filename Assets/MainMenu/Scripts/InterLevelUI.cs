using UnityEngine;
using System.Collections;

public class InterLevelUI : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;

    public GameObject button;

    private bool isLoading = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (GlobalGameManager.instance == null)
        {
            Debug.LogError("GlobalGameManager NO EXISTE en esta escena");

            winScreen.SetActive(false);
            loseScreen.SetActive(true);
            return;
        }

        bool playerWon = GlobalGameManager.instance.lastLevelWon;

        winScreen.SetActive(playerWon);
        loseScreen.SetActive(!playerWon);
    }

    public void OnButtonPressed()
    {
        if (isLoading) return;
        if (GlobalGameManager.instance == null) return;

        isLoading = true;

        StartCoroutine(HandleButton());
    }

    IEnumerator HandleButton()
    {
        yield return null;

        if (GlobalGameManager.instance.lastLevelWon)
        {
            GlobalGameManager.instance.ContinueGame();
        }
        else
        {
            GlobalGameManager.instance.ContinueGame();
        }
    }
}