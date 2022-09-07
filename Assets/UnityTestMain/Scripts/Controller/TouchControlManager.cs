using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class TouchControlManager : Singleton<TouchControlManager>
{

    public delegate void TouchMovement(AdvancedTouch.Touch fingre, int Index);
    public event TouchMovement OnTouchMove;
    public delegate void TouchStart(AdvancedTouch.Touch fingre, int Index);
    public event TouchStart OnBeginTouch;
    public delegate void TouchEnd(AdvancedTouch.Touch fingre, int Index);
    public event TouchEnd OnEndTouch;

    private void OnEnable()
    {
        AdvancedTouch.EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
        AdvancedTouch.TouchSimulation.Enable();
#endif
        AdvancedTouch.Touch.onFingerMove += OnMove;
        AdvancedTouch.Touch.onFingerDown += OnBegin;
        AdvancedTouch.Touch.onFingerUp += OnEnd;
    }
    private void OnDisable()
    {
        AdvancedTouch.EnhancedTouchSupport.Disable();
#if UNITY_EDITOR
        AdvancedTouch.TouchSimulation.Disable();
#endif
        AdvancedTouch.Touch.onFingerMove -= OnMove;
        AdvancedTouch.Touch.onFingerDown -= OnBegin;
        AdvancedTouch.Touch.onFingerUp -= OnEnd;
    }

    private void OnBegin(AdvancedTouch.Finger fingre)
    {
        OnBeginTouch?.Invoke(fingre.currentTouch, fingre.index);
    }

    public void OnEnd(AdvancedTouch.Finger fingre)
    {
        OnEndTouch?.Invoke(fingre.currentTouch, fingre.index);
    }

    public void OnMove(AdvancedTouch.Finger fingre)
    {
        OnTouchMove?.Invoke(fingre.currentTouch, fingre.index);
    }
}

