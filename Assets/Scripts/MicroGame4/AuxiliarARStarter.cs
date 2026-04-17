using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARStarter : MonoBehaviour
{
    void Start()
    {
        ARSession arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
            arSession.enabled = true;
    }
}