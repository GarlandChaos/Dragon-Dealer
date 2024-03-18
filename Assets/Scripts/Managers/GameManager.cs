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
        //Object Data
        private GameState currentGameState = GameState.NotInitialized;
        public bool hasShownTutorialScreen = false;

        //Events
        public Action<GameState> onGameStateChanged = null;

        //Properties
        public GameState CurrentGameState => currentGameState;

        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == currentGameState) return;
            currentGameState = newGameState;

            onGameStateChanged?.Invoke(newGameState);
        }
    }
}