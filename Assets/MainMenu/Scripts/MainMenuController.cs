using UnityEngine;
using TMPro;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Paneles de Inicio")]
    public GameObject splashImage;
    public GameObject mainMenuImage;
    public float splashDuration = 3f;

    [Header("Panel Final Victoria")]
    public GameObject finalScreenPanel;
    public TextMeshProUGUI finalText;
    public TextMeshProUGUI finalCoinsText;
    public TextMeshProUGUI finalMoneyText;

    void Start()
    {
        if (GlobalGameManager.instance != null && GlobalGameManager.instance.gameCompleted)
        {
            MostrarPantallaVictoria();
        }
        else
        {
            StartCoroutine(RutinaPantallaTitulo());
        }
    }

    void MostrarPantallaVictoria()
    {
        if (splashImage != null) splashImage.SetActive(false);
        if (mainMenuImage != null) mainMenuImage.SetActive(false);

        if (finalScreenPanel != null) finalScreenPanel.SetActive(true);

        if (finalText != null)
            finalText.text = "¡HAS COMPLETADO TODOS LOS MICROJUEGOS!";

        if (finalCoinsText != null)
            finalCoinsText.text = "Monedas: " + GlobalGameManager.instance.totalScore;

        if (finalMoneyText != null)
            finalMoneyText.text = "Dinero: " + (GlobalGameManager.instance.totalScore * 10000) + " €";
    }

    IEnumerator RutinaPantallaTitulo()
    {
        if (finalScreenPanel != null) finalScreenPanel.SetActive(false);

        if (splashImage != null) splashImage.SetActive(true);
        if (mainMenuImage != null) mainMenuImage.SetActive(false);

        yield return new WaitForSeconds(splashDuration);

        if (splashImage != null) splashImage.SetActive(false);
        if (mainMenuImage != null) mainMenuImage.SetActive(true);
    }
}