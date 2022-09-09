using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemSelectUIScript : MonoBehaviour
{
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentItem;
    [SerializeField] private ItemEditorUIScript m_ItemEditorUI;

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
        if(newValue == null)
        {

        }
    }
}
