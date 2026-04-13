using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        int game = GlobalGameManager.instance.currentGame;

        SceneManager.LoadScene("Microgame" + game);
    }
}