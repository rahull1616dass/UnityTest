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
    [SerializeField] private ItemScaleUIScript itemUIScale;
    [SerializeField] private ItemYAxisMove itemYMovement;
    public CurrentSelectedItemBluePrint _currentSelectedItem;

    public delegate void DefaultButtonEvent();

    public delegate void DragXZPlaneDelegate(Vector2 prevFramePos,Vector2 delta);

    public event DragXZPlaneDelegate OnDragItem;

    public delegate void ScaleAndYMovementDelegate(float deltaValue);

    public event ScaleAndYMovementDelegate OnScale;
    public event ScaleAndYMovementDelegate OnYMovement;

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
        itemUIScale.OnScale += ItemUIScale_OnScale;
        itemYMovement.OnYMovement += ItemYMovement_OnYMovement;
    }

    private void OnDisable()
    {
        itemUIEditor.OnDragItem -= ItemUIEditor_OnDragItem;
        itemUIScale.OnScale -= ItemUIScale_OnScale;
        itemYMovement.OnYMovement -= ItemYMovement_OnYMovement;
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
