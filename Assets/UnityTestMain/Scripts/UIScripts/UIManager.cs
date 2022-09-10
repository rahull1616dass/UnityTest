using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : SingletonPersistent<UIManager>
{
    [SerializeField] private Button m_AllItemButton;
    [SerializeField] private Button m_CloseItemPanel;
    [SerializeField] private ItemEditorUIScript itemUIEditor;
    public CurrentSelectedItemBluePrint _currentSelectedItem;

    public delegate void DefaultButtonEvent();

    public delegate void DragXZPlaneDelegate(Vector2 prevFramePos,Vector2 delta);

    public event DragXZPlaneDelegate OnDragItem;

    public event DefaultButtonEvent OnViewAllItem;
    public event DefaultButtonEvent OnCloseItemPanel;
    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        m_AllItemButton.onClick.AddListener(() => { OnViewAllItem?.Invoke(); });
        m_CloseItemPanel.onClick.AddListener(() => { OnCloseItemPanel?.Invoke(); });
    }

    private void OnEnable()
    {
        itemUIEditor.OnDragItem += ItemUIEditor_OnDragItem;
    }

    private void OnDisable()
    {
        itemUIEditor.OnDragItem -= ItemUIEditor_OnDragItem;
    }

    private void ItemUIEditor_OnDragItem(Vector2 prevFramePos, Vector2 newPos)
    {
        OnDragItem?.Invoke(prevFramePos, newPos);
    }
}
