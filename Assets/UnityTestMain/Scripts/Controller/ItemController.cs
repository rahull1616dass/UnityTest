using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class ItemController : MonoBehaviour
{
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentSelectedItem;
    [SerializeField] private float minScale = 0.1f;

    Camera mainCam;
    private Transform thisTransform;
    private EventTrigger thisObjectClickEvent;
    private float minY;


    private void Start()
    {
        thisTransform = transform;
        CreateAndAddTrigger();
        mainCam = Camera.main;
        minY = thisTransform.position.y;
    }

    private void OnEnable()
    {
        UIManager.Instance.OnDragItem += MoveItemOnXZPlane;
        UIManager.Instance.OnScale += OnScaleObject;
        UIManager.Instance.OnYMovement += OnYMovement;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnDragItem -= MoveItemOnXZPlane;
        UIManager.Instance.OnScale -= OnScaleObject;
        UIManager.Instance.OnYMovement -= OnYMovement;
    }

    private void OnYMovement(float deltaValueForY)
    {
        if (this != m_CurrentSelectedItem.Value)
            return;
        thisTransform.position = new Vector3(thisTransform.position.x, Math.Max(thisTransform.position.y + deltaValueForY,minY), thisTransform.position.z);
    }

    private void MoveItemOnXZPlane(Vector2 prevFramePos, Vector2 newFramePos)
    {
        if (this != m_CurrentSelectedItem.Value)
            return;
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);
        Vector3 prevFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(prevFramePos.x, prevFramePos.y, distanceFromCam));
        Vector3 newFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(newFramePos.x, newFramePos.y, distanceFromCam));
        Vector3 delta = prevFrameOnWorld - newFrameOnWorld;
        Vector3 newPos = new Vector3(thisTransform.position.x - delta.x, thisTransform.position.y, thisTransform.position.z - delta.y);
        thisTransform.position = newPos;
    }

    private void OnScaleObject(float scaleVal)
    {
        if (this != m_CurrentSelectedItem.Value)
            return;
        float currentScale = thisTransform.localScale.x;
        currentScale = Math.Max(currentScale + scaleVal, minScale);
        thisTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerClickEntry = new EventTrigger.Entry();
        onPointerClickEntry.eventID = EventTriggerType.PointerClick;
        onPointerClickEntry.callback.AddListener((eventData) => { OnClickThisObject(); });

        EventTrigger.Entry onPointerDownEntry = new EventTrigger.Entry();
        onPointerDownEntry.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry.callback.AddListener((eventData) => { GameManager.Instance.clickState = EClickState.ItemClicked; });
        thisObjectClickEvent.triggers.Add(onPointerDownEntry);
        thisObjectClickEvent.triggers.Add(onPointerClickEntry);
    }


    private void OnClickThisObject()
    {
        Debug.Log("Clicking");
        m_CurrentSelectedItem.Value = this;
    }
}
