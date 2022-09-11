using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllItemInSceneController : MonoBehaviour
{

    private ItemController currentItemController;
    private void OnEnable()
    {
        UIManager.Instance.OnDragItem += OnMoveInXZPlane;
        UIManager.Instance.OnScale += OnScaleObject;
        UIManager.Instance.OnYMovement += OnYMovement;
        UIManager.Instance.OnResetItem += OnResetItem;
        GameManager.Instance.OnClickStateChange += OnClickStateChange;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnDragItem -= OnMoveInXZPlane;
        UIManager.Instance.OnScale -= OnScaleObject;
        UIManager.Instance.OnYMovement -= OnYMovement;
        UIManager.Instance.OnResetItem -= OnResetItem;
        GameManager.Instance.OnClickStateChange -= OnClickStateChange;
    }

    private void OnClickStateChange(EClickState oldClickState, EClickState newClickState)
    {
        if (newClickState == EClickState.Default || GameManager.Instance._currentSelectedItem.Value==null)
            return;
        Debug.Log("Adding");
        SessionManager.Instance.SetSessionData();
    }

    private void OnResetItem(SessionData data)
    {
        Debug.Log("Removing");
        Transform itemToReset = data.item;
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
