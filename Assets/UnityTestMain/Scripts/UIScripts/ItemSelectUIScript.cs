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
    }

    private void OnDisable()
    {
        m_CurrentItem.ValueChanged -= OnChangedItemData;
    }

    private void OnChangedItemData(EventSource source, ItemController oldValue, ItemController newValue)
    {
        if (newValue == null)
        {
            m_ItemEditorUI.EnableOrDisableTheItemEditor(false);
            m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, newValue.gameObject));
        }
        else
        {
            m_ItemEditorUI.EnableOrDisableTheItemEditor(true);
        }
    }
}
