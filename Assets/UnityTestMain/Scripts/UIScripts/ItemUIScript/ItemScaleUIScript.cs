using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemScaleUIScript : MonoBehaviour
{
    [SerializeField] private float m_ScaleSpeed = 1f;

    private EventTrigger thisObjectClickEvent;

    public delegate void ScaleDelegate(float scaleVal);
    public event ScaleDelegate OnScale;

    private Vector2 touchPosOnPrevFrame;

    private void Start()
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
        if(touchIndex == 0 && GameManager.Instance._gameState == EGameState.ItemUIScale)
        {
            Vector2 delta = currentTouch.screenPosition - touchPosOnPrevFrame;
            OnScale?.Invoke((delta.y + delta.x) * m_ScaleSpeed);
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
        GameManager.Instance._gameState = EGameState.ItemUIScale;
        touchPosOnPrevFrame = ((PointerEventData)eventData).position;
    }
}
