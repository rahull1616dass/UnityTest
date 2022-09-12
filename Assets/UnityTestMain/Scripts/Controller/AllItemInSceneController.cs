using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// If we put the event call on the ItemController class then all of the object will fire though it is not selected, 
/// So we use this class as a manager of ItemController Class
/// </summary>

public class AllItemInSceneController : MonoBehaviour
{
    private Transform thisTransform;
    private ItemController currentItemController;
    [SerializeField] private ItemsSOBluePrint allItems;

    private void Start()
    {
        thisTransform = transform;
        GameManager.Instance._sessionhandlerInstance.DoLoadOldSession();
    }

    private void OnEnable()
    {
        GameManager.Instance._uiManagerInstance.OnDragItem += OnMoveInXZPlane;
        GameManager.Instance._uiManagerInstance.OnScale += OnScaleObject;
        GameManager.Instance._uiManagerInstance.OnYMovement += OnYMovement;
        GameManager.Instance._uiManagerInstance.OnResetItem += OnResetItem;
        GameManager.Instance._uiManagerInstance.OnDeleteItem += OnDeleteItem;
        GameManager.Instance.OnGameStateChange += OnClickStateChange;
        GameManager.Instance._sessionhandlerInstance.OnLoadOldSession += OnLoadOldSession;
        GameManager.Instance._uiManagerInstance.OnRemoveAllAsset += OnRemoveAllItems;
    }

    private void OnDisable()
    {
        GameManager.Instance._uiManagerInstance.OnDragItem -= OnMoveInXZPlane;
        GameManager.Instance._uiManagerInstance.OnScale -= OnScaleObject;
        GameManager.Instance._uiManagerInstance.OnYMovement -= OnYMovement;
        GameManager.Instance._uiManagerInstance.OnResetItem -= OnResetItem;
        GameManager.Instance._uiManagerInstance.OnDeleteItem -= OnDeleteItem;
        GameManager.Instance.OnGameStateChange -= OnClickStateChange;
        GameManager.Instance._sessionhandlerInstance.OnLoadOldSession -= OnLoadOldSession;
        GameManager.Instance._uiManagerInstance.OnRemoveAllAsset -= OnRemoveAllItems;
    }

    private void OnRemoveAllItems()
    {
        int totalChildCount = thisTransform.childCount;
        for (int i = 0; i < totalChildCount; i++)
        {
            GameObject Obj = thisTransform.GetChild(i).gameObject;
            if (!Obj.activeSelf)
                continue;
            Obj.SetActive(false);
            GameManager.Instance._sessionhandlerInstance.SaveSessionDataToLocal(Obj, EItemChangeType.DeleteAll);
            GameManager.Instance._sessionhandlerInstance.SetSessionData(Obj, EItemChangeType.DeleteAll);
        }
    }

    private void OnLoadOldSession(SerializedSessionDataList datas, Action<List<GameObject>> callBackAction)
    {
        if (datas == null)
            return;

        List<int> idOfTheObjects = new List<int>();
        datas.allDataList.Reverse();
        List<GameObject> oldObjects = new List<GameObject>();

        foreach (SerializedSessionData data in datas.allDataList)
        { 
            if (idOfTheObjects.Contains(data.id))
                continue;
            idOfTheObjects.Add(data.id);
            if (data.itemChangeType == EItemChangeType.Delete|| data.itemChangeType == EItemChangeType.DeleteAll)
                continue;
            foreach (GameItems staticItemData in allItems.StaticValue)
            {
                if (staticItemData.itemPrefab.tag == data.itemTag)
                {
                    GameObject itemObj = Instantiate(staticItemData.itemPrefab, thisTransform);
                    itemObj.transform.position = new Vector3(data.itemPosition[0], data.itemPosition[1], data.itemPosition[2]);
                    itemObj.transform.localScale = new Vector3(data.itemScale, data.itemScale, data.itemScale);
                    oldObjects.Add(itemObj);
                }
            }
        }
        callBackAction?.Invoke(oldObjects);

    }
    private void OnDeleteItem()
    {
        GameManager.Instance._currentSelectedItem.Value.gameObject.SetActive(false);
    }

    private void OnClickStateChange(EGameState oldState, EGameState newState)
    {
        SaveSessionData(oldState, newState);
        SaveSessionForNext(oldState, newState);
    }

    private void SaveSessionForNext(EGameState oldState, EGameState newState)
    {
        if (newState == EGameState.Default)
        {
            if (oldState == EGameState.ItemClicked || GameManager.Instance._currentSelectedItem.Value == null)
                return;
            GameManager.Instance._sessionhandlerInstance.SaveSessionDataToLocal(GameManager.Instance._currentSelectedItem.Value.gameObject, ConvertGameStateToChangetype(oldState));
        }
    }

    private void SaveSessionData(EGameState oldState, EGameState newState)
    {
        if (newState == EGameState.Default ||
           newState == EGameState.ItemClicked
           || GameManager.Instance._currentSelectedItem.Value == null)
            return;
        GameManager.Instance._sessionhandlerInstance.SetSessionData(GameManager.Instance._currentSelectedItem.Value.gameObject, ConvertGameStateToChangetype(newState));
    }
    private void OnResetItem(SessionData data)
    {
        //Going through loop to retrive the all items that deleted
        while (data.itemChangeType == EItemChangeType.DeleteAll)
        {
            if (data == null)
                return;
            
            data.item.SetActive(true);
            data.item.transform.position = data.itemPosition;
            data.item.transform.localScale = data.itemScale;
            GameManager.Instance._sessionhandlerInstance.SaveSessionDataToLocal(data.item, EItemChangeType.Create);

            data = GameManager.Instance._sessionhandlerInstance.sessionData;

            if (data.itemChangeType != EItemChangeType.DeleteAll)
            {
                //putting back which is poped
                GameManager.Instance._sessionhandlerInstance.SetSessionData(data.item, data.itemPosition, data.itemScale, data.itemChangeType);
                return;
            }
        }
        //if not all item deleted before then just selecting one and working with it
        GameObject itemToReset = data.item;
        if (data.itemChangeType == EItemChangeType.Delete)
            itemToReset.gameObject.SetActive(true);
        else if (data.itemChangeType == EItemChangeType.Create)
        {
            GameManager.Instance._sessionhandlerInstance.SaveSessionDataToLocal(itemToReset, EItemChangeType.Delete);
            Destroy(itemToReset);
            GameManager.Instance._currentSelectedItem.Value = null;
            return;
        }
        itemToReset.transform.position = data.itemPosition;
        itemToReset.transform.localScale = data.itemScale;


        GameManager.Instance._sessionhandlerInstance.SaveSessionDataToLocal(itemToReset, data.itemChangeType == EItemChangeType.Delete ? EItemChangeType.Create : 
                                                                                         data.itemChangeType);
    }

    private void OnYMovement(float deltaValueForY)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.MoveInY(deltaValueForY);
    }

    private void OnMoveInXZPlane(Vector2 delta)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.MoveInXZ(delta);
    }

    private void OnScaleObject(float scaleVal)
    {
        currentItemController = GameManager.Instance._currentSelectedItem.Value;
        if (currentItemController != null)
            currentItemController.Scale(scaleVal);
    }



    private EItemChangeType ConvertGameStateToChangetype(EGameState gameState)
    {
        return gameState == EGameState.ItemDelete ? EItemChangeType.Delete
            : gameState == EGameState.ItemCreated ? EItemChangeType.Create : EItemChangeType.Movement;
    }
}
