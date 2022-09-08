using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : SingletonPersistent<UIManager>
{
    [SerializeField] private Button m_AllItemButton;
    [SerializeField] private Button m_CloseItemPanel;

    public delegate void DefaultButtonEvent();

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
}
