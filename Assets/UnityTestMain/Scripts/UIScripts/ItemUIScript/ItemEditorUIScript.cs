using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEditorUIScript : MonoBehaviour
{
    [SerializeField] private RectTransform m_AreaOfEditor;
    [SerializeField] private Canvas m_CurrentCanvas;
    [SerializeField] private float minSize = 100f;

    private EventTrigger thisObjectClickEvent;

    public delegate void DragXZPlaneDelegate(Vector2 oldPos, Vector2 newPos);

    public event DragXZPlaneDelegate OnDragItem;

    private Vector2 prevFrameDragPos;

    private void Start()
    {
        CreateAndAddTrigger();
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerDrag = new EventTrigger.Entry();
        onPointerDrag.eventID = EventTriggerType.Drag;
        onPointerDrag.callback.AddListener((eventData) => { DragHandler(eventData); });
        thisObjectClickEvent.triggers.Add(onPointerDrag);

        EventTrigger.Entry onPointerDown = new EventTrigger.Entry();
        onPointerDown.eventID = EventTriggerType.PointerDown;
        onPointerDown.callback.AddListener((eventData) => { OnPointerDownToEditor(eventData); });
        thisObjectClickEvent.triggers.Add(onPointerDown);
    }

    private void OnPointerDownToEditor(BaseEventData eventData)
    {
        Debug.Log("ClickingUI");
        GameManager.Instance._gameState = EGameState.ItemUIClicked;
        PointerEventData data = (PointerEventData)eventData;
        prevFrameDragPos = data.position;
    }

    private void DragHandler(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;
        
        m_AreaOfEditor.anchoredPosition = CanvasPositioningExtensions.ScreenToCanvasPosition(m_CurrentCanvas, data.position);

        OnDragItem?.Invoke(prevFrameDragPos, data.position);
        prevFrameDragPos = data.position;
    }

    public void EnableOrDisableTheItemEditor(bool state)
    {
        gameObject.SetActive(state);
    }

    public void PositionTheUIArea(Rect areaRect)
    {
        Vector2 maxPos = CanvasPositioningExtensions.ScreenToCanvasPosition(m_CurrentCanvas, new Vector3(areaRect.xMax, areaRect.yMax, 0));
        Vector2 minPos = CanvasPositioningExtensions.ScreenToCanvasPosition(m_CurrentCanvas, new Vector3(areaRect.xMin, areaRect.yMin, 0));
        m_AreaOfEditor.anchoredPosition = new Vector2((minPos.x + maxPos.x) / 2, (minPos.y + maxPos.y) / 2);
        m_AreaOfEditor.sizeDelta = new Vector2(Mathf.Max(minSize, MathF.Abs(minPos.x - maxPos.x)), Mathf.Max(minSize, MathF.Abs(minPos.y - maxPos.y)));
    }
}
