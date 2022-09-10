using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemScaleUIScript : MonoBehaviour
{
    private EventTrigger thisObjectClickEvent;


    private void Start()
    {
        CreateAndAddTrigger();
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerDown = new EventTrigger.Entry();
        onPointerDown.eventID = EventTriggerType.PointerDown;
        onPointerDown.callback.AddListener((eventData) => { OnStartClickingButton(eventData); });
        thisObjectClickEvent.triggers.Add(onPointerDown);
    }

    private void OnStartClickingButton(BaseEventData eventData)
    {
        Debug.Log("DoScale");
    }
}
