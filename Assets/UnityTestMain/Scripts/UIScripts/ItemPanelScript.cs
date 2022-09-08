using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanelScript : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemPanel;
    private void OnEnable()
    {
        UIManager.Instance.OnViewAllItem += EnableAllItemPanel;
        UIManager.Instance.OnCloseItemPanel += CloseAllItemPanel;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnViewAllItem -= EnableAllItemPanel;
        UIManager.Instance.OnCloseItemPanel -= CloseAllItemPanel;
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
