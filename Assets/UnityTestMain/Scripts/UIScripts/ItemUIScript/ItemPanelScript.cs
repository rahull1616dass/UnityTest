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
        GameManager.Instance._uiManagerInstance.OnViewAllItem += EnableAllItemPanel;
        GameManager.Instance._uiManagerInstance.OnCloseItemPanel += CloseAllItemPanel;
        GameManager.Instance._uiManagerInstance.OnClickUndo += OnClickUndo;
    }

    private void OnDisable()
    {
        GameManager.Instance._uiManagerInstance.OnViewAllItem -= EnableAllItemPanel;
        GameManager.Instance._uiManagerInstance.OnCloseItemPanel -= CloseAllItemPanel;
        GameManager.Instance._uiManagerInstance.OnClickUndo -= OnClickUndo;
    }

    private void OnClickUndo()
    {
        SessionData dataToReset = GameManager.Instance._sessionManagerInstance.sessionData;
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
