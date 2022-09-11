using System;
using System.Collections.Generic;
using UnityEngine;

public enum EClickState
{
    None = -1,
    Default,
    MovingAround,
    ItemClicked,
    ItemUIClicked,
    ItemUIScale,
    ItemYMovement,
    ItemDelete
}

[DefaultExecutionOrder(-2)]
public class GameManager : SingletonPersistent<GameManager>
{
    private EClickState clickState = EClickState.Default;
    public CurrentSelectedItemBluePrint _currentSelectedItem;

    public EClickState _clickStateProp
    {
        get 
        { 
            return clickState;
        }
        set
        {
            OnClickStateChange?.Invoke(clickState, value);
            clickState = value;
        }
    }

    public delegate void ClickStateChangeDelegate(EClickState oldClickState, EClickState newClickState);
    public event ClickStateChangeDelegate OnClickStateChange;
}