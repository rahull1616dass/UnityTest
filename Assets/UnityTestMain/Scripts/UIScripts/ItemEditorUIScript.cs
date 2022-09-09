using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEditorUIScript : MonoBehaviour
{
    [SerializeField] private RectTransform m_AreaOfEditor;


    private EventTrigger thisObjectClickEvent;

    private void Start()
    {
        CreateAndAddTrigger();
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerMove = new EventTrigger.Entry();
        onPointerMove.eventID = EventTriggerType.Move;
        onPointerMove.callback.AddListener((eventData) => { OnMoveEditor(eventData); });
        thisObjectClickEvent.triggers.Add(onPointerMove);
    }

    private void OnMoveEditor(BaseEventData eventData)
    {
        
    }

    public void EnableOrDisableTheItemEditor(bool state)
    {
        gameObject.SetActive(state);
    }

    public void PositionTheUIArea(Rect areaRect)
    {
        m_AreaOfEditor.rect.Set(areaRect.x, areaRect.y, areaRect.width, areaRect.height);
    }


}
