using UnityEngine;
using AdvancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]


/// <summary>
/// This is taking the Unity's Touch input and creating the events
/// </summary>
public class InputManager : Singleton<InputManager>
{

    public delegate void TouchMovement(AdvancedTouch.Touch currentTouch, int touchIndex);
    public event TouchMovement OnTouchMove;
    public delegate void TouchStart(AdvancedTouch.Touch currentTouch, int touchIndex);
    public event TouchStart OnBeginTouch;
    public delegate void TouchEnd(AdvancedTouch.Touch currentTouch, int touchIndex);
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

