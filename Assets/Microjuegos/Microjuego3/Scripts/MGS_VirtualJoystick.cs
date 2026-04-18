using UnityEngine;
using UnityEngine.EventSystems;

public class MGS_VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform bgRect;
    private RectTransform handleRect;

    // El nuevo nombre que usa el Microjuego 2
    public Vector3 InputDirection { get; private set; }

    // COMPATIBILIDAD: Esta línea hace que tu Microjuego 3 siga funcionando
    public Vector2 InputVector => new Vector2(InputDirection.x, InputDirection.z);

    void Start()
    {
        bgRect = GetComponent<RectTransform>();
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

            InputDirection = new Vector3(pos.x, 0, pos.y);
            InputDirection = (InputDirection.magnitude > 1.0f) ? InputDirection.normalized : InputDirection;

            handleRect.anchoredPosition = new Vector2(InputDirection.x * (bgRect.sizeDelta.x / 2), InputDirection.z * (bgRect.sizeDelta.y / 2));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector3.zero;
        handleRect.anchoredPosition = Vector2.zero;
    }
}
