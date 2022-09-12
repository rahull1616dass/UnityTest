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
    [SerializeField] private float movementSpeed = 1f;

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

    public void MoveInY(float deltaValueForY)
    {
        thisTransform.position = new Vector3(thisTransform.position.x, Math.Max(thisTransform.position.y + deltaValueForY,minY), thisTransform.position.z);
    }

    public void MoveInXZ(Vector2 delta)
    {
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);

        Vector3 forwardDirection = mainCam.transform.forward * Mathf.Sin(40f * Mathf.PI / 180f);
        Vector3 rightDirection = mainCam.transform.right;

        forwardDirection.y = 0;
        rightDirection.y = 0;

        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;


        Vector3 relativeZDelta = delta.y * forwardDirection;
        Vector3 relativeXDelta = delta.x * rightDirection;

        Vector3 movementData = relativeXDelta + relativeZDelta;

        movementData = movementData * movementSpeed * distanceFromCam;

        thisTransform.Translate(movementData);
    }

    public void Scale(float scaleVal)
    {
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
        onPointerDownEntry.callback.AddListener((eventData) => { GameManager.Instance._gameState = EGameState.ItemClicked; });
        thisObjectClickEvent.triggers.Add(onPointerDownEntry);
        thisObjectClickEvent.triggers.Add(onPointerClickEntry);
    }


    private void OnClickThisObject()
    {
        m_CurrentSelectedItem.Value = this;
    }
}
