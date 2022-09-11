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
        GameManager.Instance._sessionManagerInstance.DoLoadOldSession();
    }

    private void OnEnable()
    {
        GameManager.Instance._uiManagerInstance.OnDragItem += OnMoveInXZPlane;
        GameManager.Instance._uiManagerInstance.OnScale += OnScaleObject;
        GameManager.Instance._uiManagerInstance.OnYMovement += OnYMovement;
        GameManager.Instance._uiManagerInstance.OnResetItem += OnResetItem;
        GameManager.Instance._uiManagerInstance.OnDeleteItem += OnDeleteItem;
        GameManager.Instance.OnGameStateChange += OnClickStateChange;
        GameManager.Instance._sessionManagerInstance.OnLoadOldSession += OnLoadOldSession;
    }

    private void OnDisable()
    {
        GameManager.Instance._uiManagerInstance.OnDragItem -= OnMoveInXZPlane;
        GameManager.Instance._uiManagerInstance.OnScale -= OnScaleObject;
        GameManager.Instance._uiManagerInstance.OnYMovement -= OnYMovement;
        GameManager.Instance._uiManagerInstance.OnResetItem -= OnResetItem;
        GameManager.Instance._uiManagerInstance.OnDeleteItem -= OnDeleteItem;
        GameManager.Instance.OnGameStateChange -= OnClickStateChange;
        GameManager.Instance._sessionManagerInstance.OnLoadOldSession -= OnLoadOldSession;
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
            if (data.itemChangeType == EItemChangeType.Delete)
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
            GameManager.Instance._sessionManagerInstance.SaveSessionDataToLocal(GameManager.Instance._currentSelectedItem.Value.gameObject, ConvertGameStateToChangetype(oldState));
        }
        

    }

    private void SaveSessionData(EGameState oldState, EGameState newState)
    {
        if (newState == EGameState.Default ||
           newState == EGameState.ItemClicked
           || GameManager.Instance._currentSelectedItem.Value == null)
            return;
        Debug.Log("Adding");
        GameManager.Instance._sessionManagerInstance.SetSessionData(ConvertGameStateToChangetype(newState));
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



    private EItemChangeType ConvertGameStateToChangetype(EGameState gameState)
    {
        return gameState == EGameState.ItemDelete ? EItemChangeType.Delete
            : gameState == EGameState.ItemCreated ? EItemChangeType.Create : EItemChangeType.Movement;
    }
}
