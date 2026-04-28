using System.Diagnostics;
using UnityEngine;

public class ARPlayerHead : MonoBehaviour
{
    [Header("Links Patateros A Otros Objetos")]
    public ARGameMaster gameMaster;

    void Update()
    {
        //transform.position = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        //transform.position = Camera.main.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.LogError("* * * * * * * * * * * * * * * * * * * * * * * TRIGGER ENTER");
        if (other.CompareTag("ARHazard"))
        {
            UnityEngine.Debug.LogError("* * * * * * * * * * * * * * * * * * * * * * * HAZARD ------------------------");
            gameMaster.AddHazardCollision();
        }

        if (other.CompareTag("ARCollectible"))
        {
            UnityEngine.Debug.LogError("* * * * * * * * * * * * * * * * * * * * * * * COLLECTIBLE ------------------------");
            gameMaster.CollectibleAdd(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnityEngine.Debug.LogError("* * * * * * * * * * * * * * * * * * * * * * * TRIGGER EXIT");
        if (other.CompareTag("ARHazard"))
        {
            UnityEngine.Debug.LogError("* * * * * * * * * * * * * * * * * * * * * * * HAZARD EXIT 888888888888888888 - - - - - ");
            gameMaster.RemoveHazardCollision();
        }
    }
}