using System;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    None = -1,
    Default,
    ItemCreated,
    ItemClicked,
    ItemUIClicked,
    ItemUIScale,
    ItemYMovement,
    ItemDelete
}

[DefaultExecutionOrder(-2)]
public class GameManager : SingletonPersistent<GameManager>
{
    private EGameState gameState = EGameState.Default;
    public CurrentSelectedItemBluePrint _currentSelectedItem;

    public SessionHandler _sessionhandlerInstance;
    public UIManager _uiManagerInstance;
    public TouchController _touchControllerInstance;

    public EGameState _gameState
    {
        get 
        { 
            return gameState;
        }
        set
        {
            OnGameStateChange?.Invoke(gameState, value);
            gameState = value;
        }
    }

    public delegate void GameStateChangeDelegate(EGameState oldState, EGameState newState);
    public event GameStateChangeDelegate OnGameStateChange;
}