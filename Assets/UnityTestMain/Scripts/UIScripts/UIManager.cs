using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : SingletonPersistent<UIManager>
{
    [SerializeField] private Button m_AllItemButton;
    [SerializeField] private Button m_CloseItemPanel;
    [SerializeField] private Button m_UndoButton;
    [SerializeField] private ItemEditorUIScript m_ItemUIEditor;
    [SerializeField] private ItemScaleUIScript m_ItemUIScale;
    [SerializeField] private ItemYAxisMove m_ItemYMovement;
    [SerializeField] private ItemPanelScript m_ItemPanel;

    public delegate void DefaultButtonEvent();

    public delegate void DragXZPlaneDelegate(Vector2 prevFramePos,Vector2 delta);

    public event DragXZPlaneDelegate OnDragItem;

    public delegate void ResetItem(SessionData data);

    public event ResetItem OnResetItem;

    public delegate void ScaleAndYMovementDelegate(float deltaValue);

    public event ScaleAndYMovementDelegate OnScale;
    public event ScaleAndYMovementDelegate OnYMovement;

    public event DefaultButtonEvent OnViewAllItem;
    public event DefaultButtonEvent OnCloseItemPanel;
    public event DefaultButtonEvent OnClickUndo;

    private void Start()
    {
        m_AllItemButton.onClick.AddListener(() => { OnViewAllItem?.Invoke(); });
        m_CloseItemPanel.onClick.AddListener(() => { OnCloseItemPanel?.Invoke(); });
        m_UndoButton.onClick.AddListener(() => { OnClickUndo?.Invoke(); });
    }

    private void OnEnable()
    {
        m_ItemUIEditor.OnDragItem += ItemUIEditor_OnDragItem;
        m_ItemUIScale.OnScale += ItemUIScale_OnScale;
        m_ItemYMovement.OnYMovement += ItemYMovement_OnYMovement;
        m_ItemPanel.OnResetItem += OnItemReset;

        SessionManager.Instance.OnEntryFirstSessionData += OnFirstMove;
        SessionManager.Instance.OnSessionDataEmpty += OnLastReset;
    }

    private void OnDisable()
    {
        m_ItemUIEditor.OnDragItem -= ItemUIEditor_OnDragItem;
        m_ItemUIScale.OnScale -= ItemUIScale_OnScale;
        m_ItemYMovement.OnYMovement -= ItemYMovement_OnYMovement;
        m_ItemPanel.OnResetItem -= OnItemReset;

        SessionManager.Instance.OnEntryFirstSessionData -= OnFirstMove;
        SessionManager.Instance.OnSessionDataEmpty -= OnLastReset;
    }

    private void OnLastReset()
    {
        m_UndoButton.interactable = false;
    }

    private void OnFirstMove()
    {
        m_UndoButton.interactable = true;
    }

    private void OnItemReset(SessionData data)
    {
        OnResetItem?.Invoke(data);
    }

    private void ItemYMovement_OnYMovement(float deltaValue)
    {
        OnYMovement?.Invoke(deltaValue);
    }

    private void ItemUIEditor_OnDragItem(Vector2 prevFramePos, Vector2 newPos)
    {
        OnDragItem?.Invoke(prevFramePos, newPos);
    }

    private void ItemUIScale_OnScale(float scaleVal)
    {
        OnScale?.Invoke(scaleVal);
    }
}
