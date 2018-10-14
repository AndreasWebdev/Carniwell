using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();

    private bool isPressed = false;

    void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.enabled = false;
    }

    void Update() {
        if (!isPressed) {
            if (Input.touchCount == 1) {
                Touch touch = Input.GetTouch(0);

                Canvas canvas = GetComponentInParent<Canvas>();
                canvas.enabled = true;

                background.position = touch.position;
                joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;

                OnPointerDown(pointerEventData);
                OnPointerUp(pointerEventData);
            }
        } else {
            Canvas canvas = GetComponentInParent<Canvas>();
            canvas.enabled = false;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.enabled = false;
    }
}