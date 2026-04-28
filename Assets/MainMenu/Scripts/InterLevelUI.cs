using UnityEngine;
using TMPro;

public class InterLevelUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI buttonText;

    public bool playerWon;

    void Start()
    {
        bool playerWon = GlobalGameManager.instance.lastLevelWon;

        if (playerWon)
        {
            messageText.text = "HAS GANADO";
            buttonText.text = "Siguiente";
        }
        else
        {
            messageText.text = "HAS PERDIDO";
            buttonText.text = "Reintentar";
        }
    }

    public void OnButtonPressed()
    {
        GlobalGameManager.instance.ContinueGame();
    }
}