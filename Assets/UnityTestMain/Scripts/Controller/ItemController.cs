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

    Camera mainCam;
    private Transform thisTransform;
    private EventTrigger thisObjectClickEvent;

    public Vector3 currentPosition { 
        get {
                if (thisTransform == null)
                    thisTransform = transform;
                return thisTransform.position; 
        } 
    }

    public Vector3 currentScale
    {
        get
        {
            if (thisTransform == null)
                thisTransform = transform;
            return thisTransform.localScale;
        }
    }

    private void Start()
    {
        thisTransform = transform;
        CreateAndAddTrigger();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        UIManager.Instance.OnDragItem += MoveItemOnXZPlane;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnDragItem -= MoveItemOnXZPlane;
    }

    private void MoveItemOnXZPlane(Vector2 prevFramePos, Vector2 newFramePos)
    {
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);
        Vector3 prevFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(prevFramePos.x, prevFramePos.y, distanceFromCam));
        Vector3 newFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(newFramePos.x, newFramePos.y, distanceFromCam));
        Vector3 delta = prevFrameOnWorld - newFrameOnWorld;
        Vector3 newPos = new Vector3(thisTransform.position.x - delta.x, thisTransform.position.y, thisTransform.position.z - delta.y);
        transform.position = newPos;
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

    public void OnMoveItem(Vector3 position)
    {

        Debug.Log("Clicking");
        thisTransform.position = position;
    }

    public void OnScaleItem(float ScaleValue)
    {
        thisTransform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
    }
}
