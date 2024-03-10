using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game.Gameplay
{
    public class LevelManager : ASingleton<LevelManager>
    {
        [Header("External Data")]
        [SerializeField] private EntityController playerControllerPrefab = null;
        [SerializeField] private List<Level> levelList = new();

        private EntityController playerController = null;
        private int currentLevel = 0;
        private int currentWave = 0;
        private int waveCount = 0;

        //Properties
        public EntityController PlayerController => playerController;
        public bool HasLevelsRemaining => currentLevel + 1 < levelList.Count;
        public int CurrentLevel => currentLevel;
        public int CurrentWave => currentWave;

        private void Start()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        private void InitializeLevel()
        {
            SpawnPlayer();
            SpawnWave();
        }

        private void SpawnPlayer()
        {
            if (playerController == null)
                playerController = Instantiate(playerControllerPrefab);

            playerController.gameObject.SetActive(true);
            playerController.Initialize();
        }

        private void SpawnWave()
        {
            if (levelList.Count == 0) return;

            if (levelList[currentLevel].WaveList.Count == 0) return;

            waveCount = levelList[currentLevel].WaveList.Count;

            List<WaveUnit> waveUnitList = levelList[currentLevel].WaveList[currentWave].WaveUnitList;
            foreach (WaveUnit waveUnit in waveUnitList)
            {
                for (int i = 0; i < waveUnit.numberOfEnemies; i++)
                {
                    EntityController enemy = EntityControllerManager.Instance.GetEntityController();
                    enemy.Initialize();
                    enemy.SetEntityElement(waveUnit.element);
                }
            }
            UIManager.Instance.RequestScreen(ScreenIds.WAVE_TITLE_SCREEN, true);
        }

        private void GoToNextWave()
        {
            currentWave++;

            if (currentWave >= waveCount)
            {
                GameManager.Instance.ChangeGameState(GameState.GameEnd);
                UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, false);
                UIManager.Instance.RequestScreen(ScreenIds.WIN_SCREEN, true);
                return;
            }

            GameManager.Instance.ChangeGameState(GameState.WaveStart);
        }

        public void GoToNextLevel()
        {
            currentLevel++;
            currentWave = 0;

            if(currentLevel >= levelList.Count) return;

            UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, true);
        }

        public void ReleaseDeadEnemy(EntityController enemy)
        {
            EntityUIControllerManager.Instance.ReleaseEntityUIController(enemy.EntityUIController);
            EntityControllerManager.Instance.ReleaseEntityController(enemy);

            if (EntityControllerManager.Instance.ActiveEntityControllerCount == 0)
                GoToNextWave();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    currentLevel = 0;
                    currentWave = 0;
                    break;
                case GameState.WaveStart:
                    InitializeLevel();
                    break;
                case GameState.GameRunning:
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    EntityControllerManager.Instance.ClearEntityControllerList();
                    EntityUIControllerManager.Instance.ClearEntityUIControllerList();
                    playerController.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}