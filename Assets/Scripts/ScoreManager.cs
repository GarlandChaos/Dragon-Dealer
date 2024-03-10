using Game.Gameplay;
using Game.Utility.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class SaveScoreData
    {
        public LevelScoreData[] scoreArray;
    }

    [Serializable]
    public class LevelScoreData
    {
        public int level = 0;
        public int score = 0;
    }

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
            List<LevelScoreData> levelScoreDataList = new();
            foreach(KeyValuePair<int, int> kvp in scoreDictionary)
            {
                LevelScoreData levelScoreData = new LevelScoreData() { level = kvp.Key, score = kvp.Value };
                levelScoreDataList.Add(levelScoreData);
            }
            SaveScoreData saveScoreData = new SaveScoreData() { scoreArray = levelScoreDataList.ToArray() };
            string json = JsonUtility.ToJson(saveScoreData);
            Debug.Log("json: " + json);

            SaveLoadAPI.Save(json);
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