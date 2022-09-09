using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class ItemController : MonoBehaviour
{
    [SerializeField] private int m_CurrentLayer, m_PlaneLayer;
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentItem;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnBeginTouch += InputManager_OnBeginTouch;
        InputManager.Instance.OnEndTouch += InputManager_OnEndTouch;
    }


    private void OnDisable()
    {
        InputManager.Instance.OnBeginTouch -= InputManager_OnBeginTouch;
        InputManager.Instance.OnEndTouch -= InputManager_OnEndTouch;
    }

    private void InputManager_OnBeginTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        if(touchIndex == 0)
        {
            Ray ray = mainCam.ScreenPointToRay(currentTouch.screenPosition);
            if (Physics.Raycast(ray.origin, ray.direction, 1 << m_CurrentLayer))
            {
                GameManager.Instance.clickState = EClickState.FirstTapOnObject;
                m_CurrentItem.Value = null;
            }
            else if (Physics.Raycast(ray.origin, ray.direction, 1 << m_PlaneLayer))
            {
                GameManager.Instance.clickState = EClickState.NormalMovement;
                m_CurrentItem.Value = null;
            }
        }
    }

    private void InputManager_OnMoveTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        if (touchIndex == 0)
        {
            Ray ray = mainCam.ScreenPointToRay(currentTouch.screenPosition);
            if (Physics.Raycast(ray.origin, ray.direction, 1 << m_CurrentLayer) && GameManager.Instance.clickState == EClickState.FirstTapOnObject)
            {
                GameManager.Instance.clickState = EClickState.NormalMovement;
                m_CurrentItem.Value = null;
            }
            else if (Physics.Raycast(ray.origin, ray.direction, 1 << m_PlaneLayer))
            {
                GameManager.Instance.clickState = EClickState.NormalMovement;
                m_CurrentItem.Value = null;
            }
        }
    }

    private void InputManager_OnEndTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        if (touchIndex == 0)
        {
            Ray ray = mainCam.ScreenPointToRay(currentTouch.screenPosition);
            if (Physics.Raycast(ray.origin, ray.direction, 1 << m_CurrentLayer) && GameManager.Instance.clickState == EClickState.FirstTapOnObject)
            {
                GameManager.Instance.clickState = EClickState.ObjectTapped;
                m_CurrentItem.Value = this;
            }
            else if (Physics.Raycast(ray.origin, ray.direction, 1 << m_PlaneLayer))
            {
                GameManager.Instance.clickState = EClickState.NormalMovement;
                m_CurrentItem.Value = null;
            }
        }
    }
}
