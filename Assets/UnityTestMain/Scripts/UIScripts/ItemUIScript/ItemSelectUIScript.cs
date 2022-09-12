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
        GameManager.Instance._currentSelectedItem.ValueChanged += OnChangedItemData;
        GameManager.Instance._uiManagerInstance.OnDragItem += AreaChange;
        GameManager.Instance._uiManagerInstance.OnScale += OnScaleOrYChange;
        GameManager.Instance._uiManagerInstance.OnYMovement += OnScaleOrYChange;
        InputManager.Instance.OnEndTouch += OnEndTouchFromScreen;
        GameManager.Instance._uiManagerInstance.OnDeleteItem += OnDeleteItem;
        GameManager.Instance._touchControllerInstance.OnMoveCam += OnCamMove;
    }

    private void OnDisable()
    {
        GameManager.Instance._currentSelectedItem.ValueChanged -= OnChangedItemData;
        GameManager.Instance._uiManagerInstance.OnDragItem -= AreaChange;
        GameManager.Instance._uiManagerInstance.OnScale -= OnScaleOrYChange;
        GameManager.Instance._uiManagerInstance.OnYMovement -= OnScaleOrYChange;
        InputManager.Instance.OnEndTouch -= OnEndTouchFromScreen;
        GameManager.Instance._uiManagerInstance.OnDeleteItem -= OnDeleteItem;
        GameManager.Instance._touchControllerInstance.OnMoveCam -= OnCamMove;
    }

    private void OnCamMove()
    {
        if (GameManager.Instance._currentSelectedItem.Value != null && m_ItemEditorUI._thisGameObject.activeSelf)
            m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, GameManager.Instance._currentSelectedItem.Value.gameObject));
    }

    private void OnDeleteItem()
    {
        m_ItemEditorUI.EnableOrDisableTheItemEditor(false);
    }

    private void OnScaleOrYChange(float scaleVal)
    {
        m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, GameManager.Instance._currentSelectedItem.Value.gameObject));
    }

    private void AreaChange(Vector2 delta)
    {
        m_ItemEditorUI.PositionTheUIArea(UIRectAreaOf3DObject.CovertObjectToRect(mainCam, GameManager.Instance._currentSelectedItem.Value.gameObject));
    }

    private void OnEndTouchFromScreen(UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch, int touchIndex)
    {
        if (GameManager.Instance._gameState == EGameState.Default)
        {
            GameManager.Instance._currentSelectedItem.Value = null;
        }
        GameManager.Instance._gameState = EGameState.Default;
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
