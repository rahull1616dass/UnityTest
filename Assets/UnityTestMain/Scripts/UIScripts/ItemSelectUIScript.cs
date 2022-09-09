using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemSelectUIScript : MonoBehaviour
{
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentItem;
    [SerializeField] private ItemEditorUIScript m_ItemEditorUI;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        m_CurrentItem.ValueChanged += OnChangedItemData;
        InputManager.Instance.OnEndTouch += OnEndTouchFromScreen;
    }

    private void OnDisable()
    {
        m_CurrentItem.ValueChanged -= OnChangedItemData;
        InputManager.Instance.OnEndTouch -= OnEndTouchFromScreen;
    }

    private void OnEndTouchFromScreen(UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch, int touchIndex)
    {
        if (!ItemController.IsClickingItemObject)
            m_CurrentItem.Value = null;
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
