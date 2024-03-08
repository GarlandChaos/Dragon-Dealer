using Game.UI;
using Game.Utility.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class LevelController : ASingleton<LevelController>
    {
        [Header("External Data")]
        [SerializeField] private List<Level> levelList = new();

        [Header("Self Contained References")]
        [SerializeField] private EntityControllerPool entityControllerPool = new();

        private int currentLevel = 0;
        private int currentWave = 0;
        private int waveCount = 0;
        private List<EntityController> enemyList = new();

        public Action<EntityController> onEnemyInstantiated = null;

        public bool HasLevelsRemaining => currentLevel + 1 < levelList.Count;

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
                EntityController enemy = entityControllerPool.Pool.Get();
                enemy.SetEntityElement(waveUnit.element);
                enemyList.Add(enemy);
                onEnemyInstantiated?.Invoke(enemy);
            }
        }

        private void GoToNextWave()
        {
            currentWave++;

            if(currentWave > waveCount)
            {
                GoToNextLevel();
            }

            SpawnWave();
        }

        public void GoToNextLevel()
        {
            currentLevel++;

            if(currentLevel > levelList.Count)
            {
                UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            }

            InitializeLevel();
        }

        public void OnEnemyDefeated(EntityController enemy)
        {
            enemyList.Remove(enemy);

            if (enemyList.Count == 0)
                GoToNextWave();
        }
    }
}