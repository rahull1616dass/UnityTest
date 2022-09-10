using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EClickState
{
    None = -1,
    Default,
    ItemClicked,
    ItemUIClicked
}

public class GameManager : Singleton<GameManager>
{
    public EClickState clickState = EClickState.Default;
}
