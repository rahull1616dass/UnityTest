using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class ItemController : MonoBehaviour
{
    [SerializeField] private CurrentSelectedItemBluePrint m_CurrentSelectedItem;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float movementSpeed = 1f;

    Camera mainCam;
    private Transform thisTransform;
    private EventTrigger thisObjectClickEvent;
    private float minY;


    private void Start()
    {
        thisTransform = transform;
        CreateAndAddTrigger();
        mainCam = Camera.main;
        minY = thisTransform.position.y;
    }

    public void MoveInY(float deltaValueForY)
    {
        thisTransform.position = new Vector3(thisTransform.position.x, Math.Max(thisTransform.position.y + deltaValueForY,minY), thisTransform.position.z);
    }

    public void MoveInXZ(Vector2 delta)
    {
        float distanceFromCam = Vector3.Distance(mainCam.transform.position, transform.position);
        //Vector3 prevFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(prevFramePos.x, prevFramePos.y, distanceFromCam));
        //Vector3 newFrameOnWorld = mainCam.ScreenToWorldPoint(new Vector3(newFramePos.x, newFramePos.y, distanceFromCam));
        delta = delta *movementSpeed*distanceFromCam;
        float cameraAngle = mainCam.transform.localEulerAngles.y > 180 ? mainCam.transform.localEulerAngles.y - 360f : mainCam.transform.localEulerAngles.y;
        float displacementRatio = cameraAngle / 90f;
        float XDisplacement=0f;
        float YDisplacement=0f;
        Debug.Log(delta);
        Debug.Log(displacementRatio);
        if (-1f <= displacementRatio && displacementRatio <= 0f)
        {
            XDisplacement = delta.x * (1 - (-(displacementRatio))) + delta.y * ((displacementRatio));
            YDisplacement = delta.y * (1 - (-(displacementRatio))) + delta.x * (-(displacementRatio));
        }
        else if (-2f < displacementRatio && displacementRatio < -1f)
        {
            XDisplacement = delta.x * (displacementRatio + 1) - delta.y * (displacementRatio + 2);
            YDisplacement = delta.y * (1 - (-(displacementRatio))) + delta.x * (displacementRatio + 2);
        }
        else if (2f > displacementRatio && displacementRatio > 1f)
        {
            XDisplacement = delta.x * (1 - displacementRatio) + delta.y * (1 - (displacementRatio - 1));
            YDisplacement = delta.y * (1 - displacementRatio) + delta.x * ((displacementRatio -1) - 1);
        }
        else if (1f >= displacementRatio && displacementRatio > 0f)
        {
            XDisplacement = delta.x * (1- displacementRatio) + delta.y * (1 - (1 - displacementRatio));
            YDisplacement = delta.y * (1 - displacementRatio) + delta.x * ((1 - displacementRatio) - 1);
        }
        Vector3 newPos = new Vector3(thisTransform.position.x + XDisplacement, thisTransform.position.y, thisTransform.position.z + YDisplacement);
        thisTransform.position = newPos;
    }

    public void Scale(float scaleVal)
    {
        float currentScale = thisTransform.localScale.x;
        currentScale = Math.Max(currentScale + scaleVal, minScale);
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
