using Game.UI;
using Game.Utility.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class LevelManager : ASingleton<LevelManager>
    {
        [Header("External Data")]
        [SerializeField] private List<Level> levelList = new();

        [Header("Self Contained References")]
        [SerializeField] private EntityControllerPool entityControllerPool = null;

        private int currentLevel = 0;
        private int currentWave = 0;
        private int waveCount = 0;
        private List<EntityController> enemyList = new();

        public Action<EntityController> onEnemyInstantiated = null;
        public Action onLevelStarted = null;
        public Action onGameWon = null;

        public bool HasLevelsRemaining => currentLevel + 1 < levelList.Count;

        public void ResetLevelIndex()
        {
            currentLevel = 0;
        }

        public void InitializeLevel()
        {
            currentWave = 0;
            waveCount = levelList[currentLevel].WaveList.Count;

            SpawnWave();
        }

        private void SpawnWave()
        {
            if (levelList.Count == 0) return;

            if (levelList[currentLevel].WaveList.Count == 0) return;

            List<WaveUnit> waveUnitList = levelList[currentLevel].WaveList[currentWave].WaveUnitList;
            foreach (WaveUnit waveUnit in waveUnitList)
            {
                for (int i = 0; i < waveUnit.numberOfEnemies; i++)
                {
                    EntityController enemy = entityControllerPool.Pool.Get();
                    enemy.SetEntityElement(waveUnit.element);
                    enemy.Initialize();
                    enemyList.Add(enemy);
                    onEnemyInstantiated?.Invoke(enemy);
                }
            }

            StartCoroutine(RaiseOnLevelStartedEventRoutine());
        }

        private void GoToNextWave()
        {
            currentWave++;

            if(currentWave >= waveCount)
            {
                GoToNextLevel();
                return;
            }

            SpawnWave();
        }

        public void GoToNextLevel()
        {
            currentLevel++;

            if(currentLevel >= levelList.Count)
            {
                UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, false);
                UIManager.Instance.RequestScreen(ScreenIds.WIN_SCREEN, true);
                onGameWon?.Invoke();
                return;
            }

            InitializeLevel();
        }

        public void ReleaseDeadEnemy(EntityController enemy)
        {
            entityControllerPool.Pool.Release(enemy);
            enemyList.Remove(enemy);

            if (enemyList.Count == 0)
                GoToNextWave();
        }

        private IEnumerator RaiseOnLevelStartedEventRoutine()
        {
            yield return new WaitForEndOfFrame();
            onLevelStarted?.Invoke();
        }
    }
}