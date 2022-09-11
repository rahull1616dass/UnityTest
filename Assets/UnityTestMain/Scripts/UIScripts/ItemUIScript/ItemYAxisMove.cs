using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemYAxisMove : MonoBehaviour
{
    [SerializeField] private float m_YMovementSpeed = 1f;
    private EventTrigger thisObjectClickEvent;

    public delegate void YMovementDelegate(float scaleVal);
    public event YMovementDelegate OnYMovement;

    private Vector2 touchPosOnPrevFrame;
    // Start is called before the first frame update
    void Start()
    {
        CreateAndAddTrigger();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnTouchMove += OnInputMove;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTouchMove -= OnInputMove;
    }

    private void OnInputMove(UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch, int touchIndex)
    {
        if (touchIndex == 0 && GameManager.Instance._clickStateProp == EClickState.ItemYMovement)
        {
            Vector2 delta = currentTouch.screenPosition - touchPosOnPrevFrame;
            OnYMovement?.Invoke(delta.y * m_YMovementSpeed);
            touchPosOnPrevFrame = currentTouch.screenPosition;
        }
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerEnter = new EventTrigger.Entry();
        onPointerEnter.eventID = EventTriggerType.PointerDown;
        onPointerEnter.callback.AddListener((eventData) => { OnPointerDown(eventData); });
        thisObjectClickEvent.triggers.Add(onPointerEnter);
    }

    private void OnPointerDown(BaseEventData eventData)
    {
        GameManager.Instance._clickStateProp = EClickState.ItemYMovement;
        touchPosOnPrevFrame = ((PointerEventData)eventData).position;
    }
}
