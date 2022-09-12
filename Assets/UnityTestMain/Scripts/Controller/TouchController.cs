using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class TouchController : MonoBehaviour
{
    [SerializeField] private Camera m_MainCam;
    private InputManager inputManager;
    private Plane thisPlane;
    private Vector3 primaryTouchDelta;
    private Vector2 primaryTouchPrevPosition, secondaryTouchPrevPosition;
    private AdvancedTouch.Touch primaryTouch, secondaryTouch;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        if (m_MainCam == null)
            m_MainCam = Camera.main;
    }

    private void OnEnable()
    {
        inputManager.OnBeginTouch += InputManager_OnBeginTouch;
        inputManager.OnEndTouch += InputManager_OnEndTouch;
        inputManager.OnTouchMove += InputManager_OnTouchMove;
    }

    private void OnDisable()
    {
        inputManager.OnBeginTouch -= InputManager_OnBeginTouch;
        inputManager.OnEndTouch -= InputManager_OnEndTouch;
        inputManager.OnTouchMove -= InputManager_OnTouchMove;
    }

    private void InputManager_OnBeginTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        AssignTouchVariables(currentTouch, touchIndex);
        thisPlane.SetNormalAndPosition(transform.up, transform.position);
        AssignVectors(currentTouch, touchIndex);
    }

    private void InputManager_OnTouchMove(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        AssignTouchVariables(currentTouch, touchIndex);
        thisPlane.SetNormalAndPosition(transform.up, transform.position);
        primaryTouchDelta = Vector3.zero;


        if (GameManager.Instance._gameState != EGameState.Default 
            &&GameManager.Instance._gameState != EGameState.ItemClicked
            )
            return;

        if(touchIndex == 0)
        {
            primaryTouchDelta = GetAreaDeltaPosition(primaryTouch);
            m_MainCam.transform.Translate(primaryTouchDelta, Space.World);
        }

        if(touchIndex == 1)
        {
            Vector3 castPositionOfPrimaryTouchCurrFrame = GetAreaPosition(primaryTouch.screenPosition);
            Vector3 castPositionOfSecondaryTouchCurrFrame = GetAreaPosition(secondaryTouch.screenPosition);
            Vector3 castPositionOfPrimaryTouchPrevFrame = GetAreaPosition(primaryTouchPrevPosition);
            Vector3 castPositionOfSecondaryTouchPrevFrame = GetAreaPosition(secondaryTouchPrevPosition);

            float zoom = Vector3.Distance(castPositionOfPrimaryTouchCurrFrame, castPositionOfSecondaryTouchCurrFrame)/
                            Vector3.Distance(castPositionOfPrimaryTouchPrevFrame, castPositionOfSecondaryTouchPrevFrame);
            if (zoom == 0 || zoom > 10)
                return;

            m_MainCam.transform.position = Vector3.LerpUnclamped(castPositionOfPrimaryTouchCurrFrame, m_MainCam.transform.position, 1 / zoom);

            if(castPositionOfSecondaryTouchCurrFrame!= castPositionOfSecondaryTouchPrevFrame)
            {
                m_MainCam.transform.RotateAround(castPositionOfPrimaryTouchCurrFrame, thisPlane.normal, Vector3.SignedAngle(
                    castPositionOfSecondaryTouchCurrFrame - castPositionOfPrimaryTouchCurrFrame,
                    castPositionOfSecondaryTouchPrevFrame - castPositionOfPrimaryTouchPrevFrame, thisPlane.normal));
            }
        }
        AssignVectors(currentTouch, touchIndex);
    }

    private void InputManager_OnEndTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        AssignTouchVariables(currentTouch, touchIndex);
        thisPlane.SetNormalAndPosition(transform.up, transform.position);

    }

    private void AssignTouchVariables(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        if (touchIndex == 0)
            primaryTouch = currentTouch;
        else if (touchIndex == 1)
            secondaryTouch = currentTouch;
    }

    private void AssignVectors(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        if (touchIndex == 0)
            primaryTouchPrevPosition = primaryTouch.screenPosition;
        else if (touchIndex == 1)
            secondaryTouchPrevPosition = secondaryTouch.screenPosition;
    }


    private Vector3 GetAreaPosition(Vector2 screenPosition)
    {
        var rayCurrentFrame = m_MainCam.ScreenPointToRay(screenPosition);
        if (thisPlane.Raycast(rayCurrentFrame, out var enterNow))
            return rayCurrentFrame.GetPoint(enterNow);
        return Vector3.zero;
    }

    private Vector3 GetAreaDeltaPosition(AdvancedTouch.Touch currentTouch)
    {
        var rayPrevFrame = m_MainCam.ScreenPointToRay(primaryTouchPrevPosition);
        var rayCurrentFrame = m_MainCam.ScreenPointToRay(currentTouch.screenPosition);
        if (thisPlane.Raycast(rayPrevFrame, out var distancePrevFrame) && thisPlane.Raycast(rayCurrentFrame, out var distanceCurrentFrame))
            return rayPrevFrame.GetPoint(distancePrevFrame) - rayCurrentFrame.GetPoint(distanceCurrentFrame);

        //not on plane
        return Vector3.zero;
    }
}
