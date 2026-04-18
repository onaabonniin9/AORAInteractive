using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonVolver : MonoBehaviour
{
    public void IrAlMenu()
    {
        // Asegúrate de que el nombre sea exactamente el de tu escena de menú
        SceneManager.LoadScene("MenuPrincipal");
    }
}
