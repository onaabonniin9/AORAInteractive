using UnityEngine;

/// <summary>
/// HapticFeedback - Wrapper para vibración táctil iOS (Taptic Engine)
/// Usa iOS native haptics si está disponible, fallback a Handheld.Vibrate en Android
/// Para iOS necesita: Settings > Player > Other Settings > Allow 'unsafe' Code = true
/// </summary>

namespace Microjuego1
{
    public static class HapticFeedback
    {
#if UNITY_IOS && !UNITY_EDITOR
    // Importar funciones nativas de iOS via DllImport
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _ImpactLight();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _ImpactMedium();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _ImpactHeavy();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _NotificationSuccess();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _NotificationError();
#endif

        /// <summary>Toque suave - para aciertos individuales</summary>
        public static void Light()
        {
#if UNITY_IOS && !UNITY_EDITOR
        _ImpactLight();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#else
            Debug.Log("[Haptic] Light");
#endif
        }

        /// <summary>Toque medio</summary>
        public static void Medium()
        {
#if UNITY_IOS && !UNITY_EDITOR
        _ImpactMedium();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#else
            Debug.Log("[Haptic] Medium");
#endif
        }

        /// <summary>Toque fuerte - para errores</summary>
        public static void Heavy()
        {
#if UNITY_IOS && !UNITY_EDITOR
        _ImpactHeavy();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#else
            Debug.Log("[Haptic] Heavy");
#endif
        }

        /// <summary>Notificación de éxito - para victoria</summary>
        public static void Success()
        {
#if UNITY_IOS && !UNITY_EDITOR
        _NotificationSuccess();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#else
            Debug.Log("[Haptic] Success");
#endif
        }

        /// <summary>Notificación de error - para derrota</summary>
        public static void Failure()
        {
#if UNITY_IOS && !UNITY_EDITOR
        _NotificationError();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#else
            Debug.Log("[Haptic] Failure");
#endif
        }
    }
}