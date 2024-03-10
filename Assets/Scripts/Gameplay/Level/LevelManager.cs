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

            EntityUIControllerManager.Instance.GetPlayerUIController().gameObject.SetActive(true);
            playerController.gameObject.SetActive(true);
            playerController.Initialize();
        }

        private void SpawnWave()
        {
            if (levelList.Count == 0) return;

            if (levelList[currentLevel].WaveList.Count == 0) return;

            waveCount = levelList[currentLevel].WaveList.Count;
            //Debug.Log("Active enemies count: " + EntityControllerManager.Instance.ActiveEntityControllerCount);
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
            //StartCoroutine(WaitForWaveSpawnCompletionRoutine());
            //GameManager.Instance.ChangeGameState(GameState.GameRunning);
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

            //Debug.Log("Entered next wave: " + currentWave);
            GameManager.Instance.ChangeGameState(GameState.WaveStart);
        }

        public void GoToNextLevel()
        {
            currentLevel++;
            currentWave = 0;

            if(currentLevel >= levelList.Count) return;

            GameManager.Instance.ChangeGameState(GameState.WaveStart);
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
                    currentLevel = 0;
                    currentWave = 0;
                    break;
                case GameState.Menu:
                    currentLevel = 0;
                    currentWave = 0;
                    break;
                case GameState.WaveStart:
                    InitializeLevel();
                    break;
                case GameState.GameRunning:
                    break;
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    EntityControllerManager.Instance.ClearEntityControllerList();
                    playerController.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private IEnumerator WaitForWaveSpawnCompletionRoutine()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            yield return wait;
            while (EntityControllerManager.Instance.ActiveEntityControllerCount < levelList[currentLevel].WaveList[currentWave].GetTotalUnitCount())
            {
                yield return wait;
            }
            GameManager.Instance.ChangeGameState(GameState.GameRunning);
        }
    }
}