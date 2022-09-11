using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelScript : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemPanel;

    public delegate void ResetItem(SessionData data);
    public event ResetItem OnResetItem;
    private void OnEnable()
    {
        UIManager.Instance.OnViewAllItem += EnableAllItemPanel;
        UIManager.Instance.OnCloseItemPanel += CloseAllItemPanel;
        UIManager.Instance.OnClickUndo += OnClickUndo;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnViewAllItem -= EnableAllItemPanel;
        UIManager.Instance.OnCloseItemPanel -= CloseAllItemPanel;
        UIManager.Instance.OnClickUndo -= OnClickUndo;
    }

    private void OnClickUndo()
    {
        SessionData dataToReset = SessionManager.Instance.sessionData;
        if (dataToReset != null)
        {
            OnResetItem?.Invoke(dataToReset);
        }
    }

    private void CloseAllItemPanel()
    {
        m_ItemPanel.SetActive(false);
    }

    private void EnableAllItemPanel()
    {
        m_ItemPanel.SetActive(true);
    }
}
