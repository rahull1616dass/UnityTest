using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreateScript : MonoBehaviour
{
    [SerializeField] private ItemsSOBluePrint m_allItemSO;
    [SerializeField] private ItemButtonScript m_ItemInstantiateButtonPrefab;
    [SerializeField] private Transform m_ButtonParentContent;


    private void Start()
    {
        CreateAllItems();
    }

    private void CreateAllItems()
    {
        foreach (GameItems item in m_allItemSO.scriptableObjectValue)
        {
            ItemButtonScript t_itemButton = Instantiate(m_ItemInstantiateButtonPrefab, m_ButtonParentContent);
            t_itemButton.CreateButton(item.itemImage, item.itemPrefab);
            t_itemButton.OnItemInstantiate += CreateItemOnScene;
        }
    }

    private void CreateItemOnScene(GameObject items)
    {
        Instantiate(items);
    }
}
