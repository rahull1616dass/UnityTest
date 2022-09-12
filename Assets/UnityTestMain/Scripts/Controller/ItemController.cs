using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;
/// <summary>
/// This used to control transform related data
/// </summary>
public class ItemController : MonoBehaviour
{
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentSelectedItem;
    [SerializeField] private float m_MinScale = 0.1f;
    [SerializeField] private float m_MovementSpeed = 1f;

    Camera mainCam;
    private Transform thisTransform;
    private EventTrigger thisObjectClickEvent;
    private float minY;
    private float camAngle;

    private const float AngleToRadianRatio = MathF.PI / 180f;


    private void Start()
    {
        thisTransform = transform;
        CreateAndAddTrigger();
        mainCam = Camera.main;
        minY = thisTransform.position.y;
        camAngle = mainCam.transform.localEulerAngles.x;
    }

    public void MoveInY(float deltaValueForY)
    {
        thisTransform.position = new Vector3(thisTransform.position.x, Math.Max(thisTransform.position.y + deltaValueForY,minY), thisTransform.position.z);
    }

    public void MoveInXZ(Vector2 delta)
    {
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);

        //taking only the forward part realted to objects
        Vector3 forwardDirection = mainCam.transform.forward * Mathf.Sin(camAngle * AngleToRadianRatio);
        Vector3 rightDirection = mainCam.transform.right;

        forwardDirection.y = 0;
        rightDirection.y = 0;

        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;


        Vector3 relativeZDelta = delta.y * forwardDirection;
        Vector3 relativeXDelta = delta.x * rightDirection;

        Vector3 movementData = relativeXDelta + relativeZDelta;

        movementData = movementData * m_MovementSpeed * distanceFromCam;

        thisTransform.Translate(movementData);
    }

    public void Scale(float scaleVal)
    {
        float currentScale = thisTransform.localScale.x;
        currentScale = Math.Max(currentScale + scaleVal, m_MinScale);
        thisTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void CreateAndAddTrigger()
    {

        thisObjectClickEvent = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onPointerClickEntry = new EventTrigger.Entry();
        onPointerClickEntry.eventID = EventTriggerType.PointerClick;
        onPointerClickEntry.callback.AddListener((eventData) => { OnClickThisObject(); });

        EventTrigger.Entry onPointerDownEntry = new EventTrigger.Entry();
        onPointerDownEntry.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry.callback.AddListener((eventData) => { GameManager.Instance._gameState = EGameState.ItemClicked; });
        thisObjectClickEvent.triggers.Add(onPointerDownEntry);
        thisObjectClickEvent.triggers.Add(onPointerClickEntry);
    }


    private void OnClickThisObject()
    {
        m_CurrentSelectedItem.Value = this;
    }
}
