using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Función para cargar los juegos por nombre de escena
    public void JugarMicrojuego1() => SceneManager.LoadScene("SampleScene");
    public void JugarMicrojuego2() => SceneManager.LoadScene("Microjuego2");
    public void JugarMicrojuego3() => SceneManager.LoadScene("Microjuego3");
    public void JugarMicrojuego4() => SceneManager.LoadScene("SceneARGame");
    public void JugarMicrojuego5() => SceneManager.LoadScene("Microjuego5");

    public void Salir()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}