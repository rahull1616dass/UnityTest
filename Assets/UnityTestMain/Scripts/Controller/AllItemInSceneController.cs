using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllItemInSceneController : MonoBehaviour
{
    private Transform thisTransform;
    private ItemController currentItemController;

    private void Start()
    {
        thisTransform = transform;    
    }

    private void OnEnable()
    {
        UIManager.Instance.OnDragItem += OnMoveInXZPlane;
        UIManager.Instance.OnScale += OnScaleObject;
        UIManager.Instance.OnYMovement += OnYMovement;
        UIManager.Instance.OnResetItem += OnResetItem;
        UIManager.Instance.OnDeleteItem += OnDeleteItem;
        GameManager.Instance.OnClickStateChange += OnClickStateChange;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnDragItem -= OnMoveInXZPlane;
        UIManager.Instance.OnScale -= OnScaleObject;
        UIManager.Instance.OnYMovement -= OnYMovement;
        UIManager.Instance.OnResetItem -= OnResetItem;
        UIManager.Instance.OnDeleteItem -= OnDeleteItem;
        GameManager.Instance.OnClickStateChange -= OnClickStateChange;
    }

    private void OnDeleteItem()
    {
        GameManager.Instance._currentSelectedItem.Value.gameObject.SetActive(false);
    }

    private void OnClickStateChange(EClickState oldClickState, EClickState newClickState)
    {
        if (newClickState == EClickState.Default ||
            newClickState == EClickState.ItemClicked
            || GameManager.Instance._currentSelectedItem.Value==null)
            return;
        Debug.Log("Adding");
        SessionManager.Instance.SetSessionData(newClickState == EClickState.ItemDelete ? EItemChangeType.Delete:EItemChangeType.Movement);
    }

    private void OnResetItem(SessionData data)
    {
        Debug.Log("Removing");
        Transform itemToReset = data.item;
        if (data.itemChangeType == EItemChangeType.Delete)
            itemToReset.gameObject.SetActive(true);
        itemToReset.position = data.itemPosition;
        itemToReset.localScale = data.itemScale;
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
