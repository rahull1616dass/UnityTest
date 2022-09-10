using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemSelectUIScript : MonoBehaviour
{
    [SerializeField] private ItemEditorUIScript m_ItemEditorUI;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        UIManager.Instance._currentSelectedItem.ValueChanged += OnChangedItemData;
        UIManager.Instance.OnDragItem += AreaChange;
        InputManager.Instance.OnEndTouch += OnEndTouchFromScreen;
    }

    private void OnDisable()
    {
        UIManager.Instance._currentSelectedItem.ValueChanged -= OnChangedItemData;
        UIManager.Instance.OnDragItem -= AreaChange;
        InputManager.Instance.OnEndTouch -= OnEndTouchFromScreen;
    }

    private void AreaChange(Vector2 prevFramePos, Vector2 delta)
    {
        m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, UIManager.Instance._currentSelectedItem.Value.gameObject));
    }

    private void OnEndTouchFromScreen(UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch, int touchIndex)
    {
        if (GameManager.Instance.clickState != EClickState.ItemClicked&& GameManager.Instance.clickState!= EClickState.ItemUIClicked)
            UIManager.Instance._currentSelectedItem.Value = null;
        GameManager.Instance.clickState = EClickState.Default;
    }

    private void OnChangedItemData(EventSource source, ItemController oldValue, ItemController newValue)
    {
        if (newValue == null)
        {
            m_ItemEditorUI.EnableOrDisableTheItemEditor(false);
        }
        else
        {
            m_ItemEditorUI.EnableOrDisableTheItemEditor(true);
            m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, newValue.gameObject));
        }
    }


}
