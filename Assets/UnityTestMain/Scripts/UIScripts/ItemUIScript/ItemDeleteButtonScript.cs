using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDeleteButtonScript : MonoBehaviour
{
    private EventTrigger thisObjectClickEvent;

    public delegate void ItemDeleteDeligate();
    public event ItemDeleteDeligate OnDelete;

    void Start()
    {
        CreateAndAddTrigger();
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
        GameManager.Instance._gameState = EGameState.ItemDelete;
        OnDelete?.Invoke();
    }
}
