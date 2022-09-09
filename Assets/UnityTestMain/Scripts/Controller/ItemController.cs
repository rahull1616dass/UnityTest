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

    public static bool IsClickingItemObject;

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
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerClickEntry = new EventTrigger.Entry();
        onPointerClickEntry.eventID = EventTriggerType.PointerClick;
        onPointerClickEntry.callback.AddListener((eventData) => { OnClickThisObject(); });

        EventTrigger.Entry onPointerDownEntry = new EventTrigger.Entry();
        onPointerDownEntry.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry.callback.AddListener((eventData) => { IsClickingItemObject = true; });
        thisObjectClickEvent.triggers.Add(onPointerDownEntry);
        thisObjectClickEvent.triggers.Add(onPointerClickEntry);
    }


    private void OnClickThisObject()
    {
        m_CurrentSelectedItem.Value = this;
        IsClickingItemObject = false;
    }

    public void OnMoveItem(Vector3 position)
    {
        thisTransform.position = position;
    }

    public void OnScaleItem(float ScaleValue)
    {
        thisTransform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
    }
}
