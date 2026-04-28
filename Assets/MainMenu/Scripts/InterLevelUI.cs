using UnityEngine;
using TMPro;

public class InterLevelUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        Time.timeScale = 1f;

        if (GlobalGameManager.instance == null)
        {
            Debug.LogError("GlobalGameManager NO EXISTE en esta escena");
            messageText.text = "ERROR DE SISTEMA";
            buttonText.text = "Volver al menú";
            return;
        }

        bool playerWon = GlobalGameManager.instance.lastLevelWon;

        if (playerWon)
        {
            messageText.text = "¡Microjuego Completado!";
            buttonText.text = "Siguiente Microjuego";
        }
        else
        {
            messageText.text = "Microjuego NO Superado :(";
            buttonText.text = "Reintentar";
        }
    }

    public void OnButtonPressed()
    {
        if (GlobalGameManager.instance != null)
            GlobalGameManager.instance.ContinueGame();
    }
}