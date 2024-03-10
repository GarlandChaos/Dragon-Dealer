using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScoreManager : ASingleton<ScoreManager>
    {
        private Dictionary<int, int> scoreDictionary = new();

        private void Start()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        public void AddPointsToScore(int level, int points)
        {
            if (scoreDictionary.ContainsKey(level))
                scoreDictionary[level] += points;
            else
                scoreDictionary.Add(level, points);
        }

        public void SaveScore()
        {

        }

        public int GetLevelScore(int level)
        {
            if (!scoreDictionary.ContainsKey(level)) return 0;

            return scoreDictionary[level];
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    break;
                case GameState.WaveStart:
                    if(LevelManager.Instance.CurrentWave == 0)
                        scoreDictionary.Clear();
                    break;
                case GameState.GameRunning:
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    SaveScore();
                    break;
                default:
                    break;
            }
        }
    }
}