using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlexiblePosJoyStick : Joystick
{
    [SerializeField] private FixedJoystick joystick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Camera.main == null) return;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystick.transform.position = pos;
        joystick.Show();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        joystick.Hide();
    }
}