using System.Collections;
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
    ItemYMovement
}

public class GameManager : Singleton<GameManager>
{
    public EClickState clickState = EClickState.Default;
}
