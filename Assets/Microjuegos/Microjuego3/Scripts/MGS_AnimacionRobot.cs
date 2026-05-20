using UnityEngine;

namespace Microjuego3_MGS
{
    public class MGS_AnimacionRobot : MonoBehaviour
    {
        private Animator anim;
        private Vector3 posicionAnterior;
        private Transform padre;

        void Start()
        {
            anim = GetComponent<Animator>();
            padre = transform.parent; // Coge automáticamente al MGS_Player
            
            if (padre != null)
            {
                posicionAnterior = padre.position;
            }
        }

        void Update()
        {
            if (padre != null && anim != null)
            {
                // Calculamos cuánto se ha movido físicamente desde el último frame
                float distanciaMovida = Vector3.Distance(
                    new Vector3(padre.position.x, 0, padre.position.z), 
                    new Vector3(posicionAnterior.x, 0, posicionAnterior.z)
                );
                
                // Convertimos la distancia a velocidad real
                float velocidadReal = distanciaMovida / Time.deltaTime;
                posicionAnterior = padre.position;
                
                // Le mandamos el dato al Animator
                anim.SetFloat("Velocidad", velocidadReal);
            }
        }
    }
}