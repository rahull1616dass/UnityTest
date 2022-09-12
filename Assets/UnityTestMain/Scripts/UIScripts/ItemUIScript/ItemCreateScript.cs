using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class reads the allItems Scriptable object and create the UI which can be used to create Item in scene
/// </summary>
public class ItemCreateScript : MonoBehaviour
{
    [SerializeField] private ItemsSOBluePrint m_allItemSO;
    [SerializeField] private ItemButtonScript m_ItemInstantiateButtonPrefab;
    [SerializeField] private Transform m_ButtonParentContent;
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentItem;
    [SerializeField] private Transform m_ItemTransform;


    private void Start()
    {
        CreateAllItems();
    }

    private void CreateAllItems()
    {
        foreach (GameItems item in m_allItemSO.StaticValue)
        {
            ItemButtonScript t_itemButton = Instantiate(m_ItemInstantiateButtonPrefab, m_ButtonParentContent);
            t_itemButton.CreateButton(item.itemImage, item.itemPrefab);
            t_itemButton.OnItemInstantiate += CreateItemOnScene;
        }
    }

    private void CreateItemOnScene(ItemController items)
    {
        m_CurrentItem.Value = Instantiate(items, m_ItemTransform);
        GameManager.Instance._gameState = EGameState.ItemCreated;
    }
}
