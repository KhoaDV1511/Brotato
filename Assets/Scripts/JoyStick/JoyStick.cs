using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private FixedJoystick joystick;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Camera.main == null) return;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystick.transform.position = pos;
        joystick.Show();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.Hide();
    }
}