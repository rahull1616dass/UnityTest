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

    private void OnLoadOldSession(SessionDataSOBluePrint datas)
    {
        if (datas.Value == null)
            return;
        int TotalData = datas.Value.Count;
        for (int i = 0; i < TotalData; i++)
        {
            SessionData data = datas.Value.Pop();
            GameObject obj = Instantiate(data.item, thisTransform);
            obj.transform.position = data.itemPosition;
            obj.transform.localScale = data.itemScale;
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
