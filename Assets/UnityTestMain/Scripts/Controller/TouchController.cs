using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class TouchController : MonoBehaviour
{
    [SerializeField] private Camera m_MainCam;
    private InputManager inputManager;
    private Plane thisPlane;
    private Vector3 primaryTouchDelta, secondaryTouchDelta;
    private AdvancedTouch.Touch primaryTouch, SecondaryTouch;

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
    }

    private void InputManager_OnBeginTouch(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        AssignTouchVariables(currentTouch, touchIndex);
        thisPlane.SetNormalAndPosition(transform.up, transform.position);
        primaryTouchDelta = Vector3.zero;
        secondaryTouchDelta = Vector3.zero;
    }

    private void InputManager_OnTouchMove(AdvancedTouch.Touch currentTouch, int touchIndex)
    {
        AssignTouchVariables(currentTouch, touchIndex);
        thisPlane.SetNormalAndPosition(transform.up, transform.position);
        primaryTouchDelta = Vector3.zero;
        secondaryTouchDelta = Vector3.zero;
        if(touchIndex == 0)
        {
            primaryTouchDelta = GetAreaDeltaPosition(currentTouch);
            m_MainCam.transform.Translate(primaryTouchDelta, Space.World);
        }

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
            SecondaryTouch = currentTouch;
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
        Debug.Log("Pos<>>"+currentTouch.screenPosition);
        Vector2 delta = currentTouch.screenPosition - currentTouch.screen.position.ReadValueFromPreviousFrame();
        Debug.Log("DeltaPos>>"+ delta);
        var rayPrevFrame = m_MainCam.ScreenPointToRay(currentTouch.screen.position.ReadValueFromPreviousFrame());
        var rayCurrentFrame = m_MainCam.ScreenPointToRay(currentTouch.screenPosition);
        if (thisPlane.Raycast(rayPrevFrame, out var distancePrevFrame) && thisPlane.Raycast(rayCurrentFrame, out var distanceCurrentFrame))
            return rayPrevFrame.GetPoint(distancePrevFrame) - rayCurrentFrame.GetPoint(distanceCurrentFrame);

        //not on plane
        return Vector3.zero;
    }
}
