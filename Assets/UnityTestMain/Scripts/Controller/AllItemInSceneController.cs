using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class AllItemInSceneController : MonoBehaviour
{
    private Transform thisTransform;
    private ItemController currentItemController;
    [SerializeField] private ItemsSOBluePrint allItems;

    private void Start()
    {
        thisTransform = transform;
        SessionManager.Instance.DoLoadOldSession();
    }

    private void OnEnable()
    {
        UIManager.Instance.OnDragItem += OnMoveInXZPlane;
        UIManager.Instance.OnScale += OnScaleObject;
        UIManager.Instance.OnYMovement += OnYMovement;
        UIManager.Instance.OnResetItem += OnResetItem;
        UIManager.Instance.OnDeleteItem += OnDeleteItem;
        GameManager.Instance.OnClickStateChange += OnClickStateChange;
        SessionManager.Instance.OnLoadOldSession += OnLoadOldSession;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnDragItem -= OnMoveInXZPlane;
        UIManager.Instance.OnScale -= OnScaleObject;
        UIManager.Instance.OnYMovement -= OnYMovement;
        UIManager.Instance.OnResetItem -= OnResetItem;
        UIManager.Instance.OnDeleteItem -= OnDeleteItem;
        GameManager.Instance.OnClickStateChange -= OnClickStateChange;
        SessionManager.Instance.OnLoadOldSession -= OnLoadOldSession;
    }

    private void OnLoadOldSession(SerializedSessionDataList datas)
    {
        if (datas == null)
            return;

        List<int> idOfTheObjects = new List<int>();
        datas.allDataList.Reverse();

        foreach (SerializedSessionData data in datas.allDataList)
        { 
            if (idOfTheObjects.Contains(data.id))
                continue;
            idOfTheObjects.Add(data.id);
            if (data.itemChangeType == EItemChangeType.Delete)
                continue;
            foreach (GameItems staticItemData in allItems.StaticValue)
            {
                if (staticItemData.itemPrefab.tag == data.itemTag)
                {
                    GameObject itemObj = Instantiate(staticItemData.itemPrefab, thisTransform);
                    itemObj.transform.position = new Vector3(data.itemPosition[0], data.itemPosition[1], data.itemPosition[2]);
                    itemObj.transform.localScale = new Vector3(data.itemScale, data.itemScale, data.itemScale);
                }
            }
        }
    }
    private void OnDeleteItem()
    {
        GameManager.Instance._currentSelectedItem.Value.gameObject.SetActive(false);
    }

    private void OnClickStateChange(EGameState oldClickState, EGameState newClickState)
    {
        if (newClickState == EGameState.Default ||
            newClickState == EGameState.ItemClicked
            || GameManager.Instance._currentSelectedItem.Value==null)
            return;
        Debug.Log("Adding");
        SessionManager.Instance.SetSessionData(newClickState == EGameState.ItemDelete ? EItemChangeType.Delete
            : newClickState == EGameState.ItemCreated ? EItemChangeType.Create : EItemChangeType.Movement);
    }

    private void OnResetItem(SessionData data)
    {
        Debug.Log("Removing");
        GameObject itemToReset = data.item;
        if (data.itemChangeType == EItemChangeType.Delete)
            itemToReset.gameObject.SetActive(true);
        else if (data.itemChangeType == EItemChangeType.Create)
        {
            Destroy(itemToReset);
            GameManager.Instance._currentSelectedItem.Value = null;
            return;
        }
        itemToReset.transform.position = data.itemPosition;
        itemToReset.transform.localScale = data.itemScale;
    }

    private void OnYMovement(float deltaValueForY)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.MoveInY(deltaValueForY);
    }

    private void OnMoveInXZPlane(Vector2 prevFramePos, Vector2 newFramePos)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.MoveInXZ(prevFramePos, newFramePos);
    }

    private void OnScaleObject(float scaleVal)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.Scale(scaleVal);
    }
}
