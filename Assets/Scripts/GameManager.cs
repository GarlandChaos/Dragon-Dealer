using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Game.Gameplay;
using System;

namespace Game
{
    public enum GameState
    {
        NotInitialized,
        Menu,
        WaveStart,
        GameRunning,
        GamePause,
        GameEnd,
    }

    public class GameManager : ASingleton<GameManager>
    {
        private GameState currentGameState = GameState.NotInitialized;
        public Action<GameState> onGameStateChanged = null;

        protected override void Awake()
        {
            base.Awake();
        }

        //public EntityController GetInitializedPlayer()
        //{
        //    if(playerController == null)
        //        playerController = Instantiate(playerControllerPrefab);
            
        //    playerController.Initialize();
        //    return playerController;
        //}

        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == currentGameState) return;

            currentGameState = newGameState;
            Debug.Log("Changed to game state: " + currentGameState);
            onGameStateChanged?.Invoke(newGameState);
        }
    }
}