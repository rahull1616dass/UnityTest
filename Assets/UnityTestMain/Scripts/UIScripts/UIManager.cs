using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button m_AllItemButton;
    [SerializeField] private Button m_CloseItemPanel;
    [SerializeField] private Button m_UndoButton;
    [SerializeField] private ItemEditorUIScript m_ItemUIEditorInstance;
    [SerializeField] private ItemScaleUIScript m_ItemUIScaleInstance;
    [SerializeField] private ItemYAxisMove m_ItemYMovementInstance;
    [SerializeField] private ItemPanelScript m_ItemPanelInstance;
    [SerializeField] private ItemDeleteButtonScript m_DeleteButtonScriptInstance;

    public delegate void DefaultButtonDelegate();

    public delegate void DragXZPlaneDelegate(Vector2 prevFramePos,Vector2 delta);

    public event DragXZPlaneDelegate OnDragItem;

    public delegate void ResetItem(SessionData data);

    public event ResetItem OnResetItem;

    public delegate void ScaleAndYMovementDelegate(float deltaValue);

    public event ScaleAndYMovementDelegate OnScale;
    public event ScaleAndYMovementDelegate OnYMovement;

    public event DefaultButtonDelegate OnViewAllItem;
    public event DefaultButtonDelegate OnCloseItemPanel;
    public event DefaultButtonDelegate OnClickUndo;
    public event DefaultButtonDelegate OnDeleteItem;


    private void Start()
    {
        m_AllItemButton.onClick.AddListener(() => { OnViewAllItem?.Invoke(); });
        m_CloseItemPanel.onClick.AddListener(() => { OnCloseItemPanel?.Invoke(); });
        m_UndoButton.onClick.AddListener(() => { OnClickUndo?.Invoke(); });
    }

    private void OnEnable()
    {
        m_ItemUIEditorInstance.OnDragItem += ItemUIEditor_OnDragItem;
        m_ItemUIScaleInstance.OnScale += ItemUIScale_OnScale;
        m_ItemYMovementInstance.OnYMovement += ItemYMovement_OnYMovement;
        m_ItemPanelInstance.OnResetItem += OnItemReset;
        m_DeleteButtonScriptInstance.OnDelete += OnItemDelete;

        GameManager.Instance._sessionManagerInstance.OnEntryFirstSessionData += OnFirstMove;
        GameManager.Instance._sessionManagerInstance.OnSessionDataEmpty += OnLastReset;
    }

    private void OnDisable()
    {
        m_ItemUIEditorInstance.OnDragItem -= ItemUIEditor_OnDragItem;
        m_ItemUIScaleInstance.OnScale -= ItemUIScale_OnScale;
        m_ItemYMovementInstance.OnYMovement -= ItemYMovement_OnYMovement;
        m_ItemPanelInstance.OnResetItem -= OnItemReset;
        m_DeleteButtonScriptInstance.OnDelete -= OnItemDelete;

        GameManager.Instance._sessionManagerInstance.OnEntryFirstSessionData -= OnFirstMove;
        GameManager.Instance._sessionManagerInstance.OnSessionDataEmpty -= OnLastReset;
    }

    private void OnItemDelete()
    {
        OnDeleteItem?.Invoke();
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
