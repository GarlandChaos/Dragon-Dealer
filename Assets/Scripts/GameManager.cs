using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace Game
{
    public enum GameState
    {
        NotInitialized,
        MainMenu,
        WaveStart,
        GameRunning,
        GamePause,
        GameEnd,
    }

    public class GameManager : ASingleton<GameManager>
    {
        private GameState currentGameState = GameState.NotInitialized;
        public Action<GameState> onGameStateChanged = null;

        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == currentGameState) return;

            currentGameState = newGameState;
            Debug.Log("Changed to game state: " + currentGameState);
            onGameStateChanged?.Invoke(newGameState);
        }
    }
}