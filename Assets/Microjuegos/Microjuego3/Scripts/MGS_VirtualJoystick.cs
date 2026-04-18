using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar toques en la UI

namespace Microjuego3_MGS
{
    public class MGS_VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private RectTransform bgRect;
        private RectTransform handleRect;
        public Vector2 InputVector { get; private set; }

        void Start()
        {
            bgRect = GetComponent<RectTransform>();
            // Coge la imagen hija (la palanca)
            handleRect = transform.GetChild(0).GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgRect, eventData.position, eventData.pressEventCamera, out pos))
            {
                pos.x = (pos.x / bgRect.sizeDelta.x) * 2;
                pos.y = (pos.y / bgRect.sizeDelta.y) * 2;

                InputVector = new Vector2(pos.x, pos.y);
                InputVector = (InputVector.magnitude > 1.0f) ? InputVector.normalized : InputVector;

                // Mueve visualmente la palanca
                handleRect.anchoredPosition = new Vector2(InputVector.x * (bgRect.sizeDelta.x / 2), InputVector.y * (bgRect.sizeDelta.y / 2));
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Resetea el joystick al soltar
            InputVector = Vector2.zero;
            handleRect.anchoredPosition = Vector2.zero;
        }
    }
}
