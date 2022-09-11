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

    public void MoveInY(float deltaValueForY)
    {
        thisTransform.position = new Vector3(thisTransform.position.x, Math.Max(thisTransform.position.y + deltaValueForY,minY), thisTransform.position.z);
    }

    public void MoveInXZ(Vector2 prevFramePos, Vector2 newFramePos)
    {
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);
        Vector3 prevFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(prevFramePos.x, prevFramePos.y, distanceFromCam));
        Vector3 newFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(newFramePos.x, newFramePos.y, distanceFromCam));
        Vector3 delta = prevFrameOnWorld - newFrameOnWorld;
        Vector3 newPos = new Vector3(thisTransform.position.x - delta.x, thisTransform.position.y, thisTransform.position.z - delta.y);
        thisTransform.position = newPos;
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
        onPointerDownEntry.callback.AddListener((eventData) => { GameManager.Instance._clickStateProp = EClickState.ItemClicked; });
        thisObjectClickEvent.triggers.Add(onPointerDownEntry);
        thisObjectClickEvent.triggers.Add(onPointerClickEntry);
    }


    private void OnClickThisObject()
    {
        m_CurrentSelectedItem.Value = this;
    }
}
