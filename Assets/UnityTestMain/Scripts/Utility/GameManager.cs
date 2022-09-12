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
    /// <summary>
    /// This is tracking the main states of the game
    /// </summary>
    private EGameState gameState = EGameState.Default;

    /// <summary>
    /// Tracking the current item
    /// </summary>
    public CurrentSelectedItemBluePrint _currentSelectedItem;


    /// <summary>
    /// saving the instances of the other main class
    /// </summary>
    public SessionHandler _sessionhandlerInstance;
    public UIManager _uiManagerInstance;
    public TouchController _touchControllerInstance;




    /// <summary>
    /// Using this property to trigger the event
    /// </summary>
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

    /// <summary>
    /// This is used to trigger any call upon state change
    /// </summary>
    public delegate void GameStateChangeDelegate(EGameState oldState, EGameState newState);
    public event GameStateChangeDelegate OnGameStateChange;
}